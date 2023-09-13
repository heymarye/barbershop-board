<template>
  <div class="py-5 px-8">
    <Toolbar class="mb-4">
      <template #start>
        <Button
          label="New"
          icon="pi pi-plus"
          class="p-button-success mr-2"
          @click="openDialog"
        />
      </template>
    </Toolbar>
    <DataTable
      :value="bookings"
      :paginator="true"
      :rows="10"
      :rowsPerPageOptions="[10, 20, 50]"
      responsiveLayout="scroll"
      paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown"
      currentPageReportTemplate="Showing {first} to {last} of {totalRecords} bookings"
    >
      <Column header="Barber" field="hairdresserId" :sortable="true">
        <template #body="slotProps">
          <div>
            {{ hairdressers.at(slotProps.data.hairdresserId - 1)?.firstName }}
            {{ hairdressers.at(slotProps.data.hairdresserId - 1)?.lastName }}
          </div>
        </template>
      </Column>
      <Column header="Service" field="serviceId" :sortable="true">
        <template #body="slotProps">
          <div>
            {{ services.at(slotProps.data.serviceId - 1)?.title }}
          </div>
        </template>
      </Column>
      <Column header="Date" field="date" :sortable="true"></Column>
      <Column header="Time" field="time"></Column>
      <Column header="Duration" field="serviceId">
        <template #body="slotProps">
          <div>
            {{ services.at(slotProps.data.serviceId - 1)?.duration }}
          </div>
        </template>
      </Column>
      <Column header="Discount" field="specialOfferId">
        <template #body="slotProps">
          <div>
            {{
              slotProps.data.specialOfferId > 0
                ? specialOffers.at(slotProps.data.specialOfferId - 1)?.code
                : '-'
            }}
          </div>
        </template>
      </Column>
      <Column header="Discount (%)" field="specialOfferId">
        <template #body="slotProps">
          <div>
            {{
              slotProps.data.specialOfferId > 0
                ? specialOffers.at(slotProps.data.specialOfferId - 1)
                    ?.percentage + '%'
                : '-'
            }}
          </div>
        </template>
      </Column>
      <Column header="Price" field="serviceId">
        <template #body="slotProps">
          <div v-if="slotProps.data.specialOfferId > 0">
            {{
              countTotalPrice(
                services.at(slotProps.data.serviceId - 1)?.price!,
                specialOffers.at(slotProps.data.specialOfferId - 1)?.percentage!,
              )
            }}€
          </div>
          <div v-else>
            {{
              countTotalPrice(
                services.at(slotProps.data.serviceId - 1)?.price!,
                0,
              )
            }}€
          </div>
        </template>
      </Column>
      <Column :exportable="false">
        <template #body="id">
          <Button
            icon="pi pi-pencil"
            class="p-button-rounded p-button-success p-button-text mr-2"
            @click="editBookings(id.data)"
          />
          <Button
            icon="pi pi-trash"
            class="p-button-rounded p-button-warning p-button-text"
            @click="confirmDeleteDialog(id.data)"
          />
        </template>
      </Column>
    </DataTable>
  </div>

  <Dialog
    :visible="dialog"
    header="New"
    :modal="true"
    @update:visible="hideDialog"
  >
    <div class="field">
      <label for="hairdresser">Barber</label>
      <br />
      <select
        v-model="booking.hairdresserId"
        style="border: 0.5px solid; height: 40px; margin-bottom: 10px"
      >
        <option v-for="option in hairdressers" :value="option.id">
          {{ option.firstName }} {{ option.lastName }}
        </option>
      </select>
    </div>
    <div class="field">
      <label for="service">Service</label>
      <br />
      <select
        v-model="booking.serviceId"
        style="border: 0.5px solid; height: 40px; margin-bottom: 10px"
      >
        <option v-for="option in services" :value="option.id">
          {{ option.title }}
        </option>
      </select>
    </div>
    <div class="field">
      <label for="specialOffer">Discount</label>
      <br />
      <select
        v-model="booking.specialOfferId"
        style="border: 0.5px solid; height: 40px; margin-bottom: 10px"
      >
        <option :value="null">-</option>
        <option v-for="option in specialOffers" :value="option.id">
          {{ option.code }} ({{ specialOffers.at(option.id - 1)?.percentage }}%)
        </option>
      </select>
    </div>
    <div class="field">
      <label for="date">Date</label>
      <input
        id="date"
        class="inputfield w-full"
        type="date"
        style="border: 0.5px solid; height: 40px; margin-bottom: 10px"
        v-model="booking.date"
        required
        placeholder=""
      />
    </div>
    <div class="field">
      <label for="time">Time</label>
      <input
        id="time"
        class="inputfield w-full"
        type="time"
        style="border: 0.5px solid; height: 40px; margin-bottom: 10px"
        v-model="booking.time"
        required
        placeholder=""
      />
    </div>
    <template #footer>
      <Button
        @click="
          createBooking();
          hideDialog();
        "
        >Add</Button
      >
      <Button @click="hideDialog">Cancel</Button>
    </template>
  </Dialog>

  <Dialog
    :visible="editDialog"
    header="Edit"
    :modal="true"
    @update:visible="editDialog = false"
  >
    <div class="field">
      <label for="hairdresser">Barber</label>
      <br />
      <select
        v-model="booking.hairdresserId"
        style="border: 0.5px solid; height: 40px; margin-bottom: 10px"
      >
        <option v-for="option in hairdressers" :value="option.id">
          {{ option.firstName }} {{ option.lastName }}
        </option>
      </select>
    </div>
    <div class="field">
      <label for="service">Service</label>
      <br />
      <select
        v-model="booking.serviceId"
        style="border: 0.5px solid; height: 40px; margin-bottom: 10px"
      >
        <option v-for="option in services" :value="option.id">
          {{ option.title }}
        </option>
      </select>
    </div>
    <div class="field">
      <label for="specialOffer">Discount</label>
      <br />
      <select
        v-model="booking.specialOfferId"
        style="border: 0.5px solid; height: 40px; margin-bottom: 10px"
      >
        <option :value="null">-</option>
        <option v-for="option in specialOffers" :value="option.id">
          {{ option.code }} ({{ specialOffers.at(option.id - 1)?.percentage }}%)
        </option>
      </select>
    </div>
    <div class="field">
      <label for="date">Date</label>
      <input
        id="date"
        class="inputfield w-full"
        type="date"
        style="border: 0.5px solid; height: 40px; margin-bottom: 10px"
        v-model="booking.date"
        required
        placeholder=""
      />
    </div>
    <div class="field">
      <label for="time">Time</label>
      <input
        id="time"
        class="inputfield w-full"
        type="time"
        style="border: 0.5px solid; height: 40px; margin-bottom: 10px"
        v-model="booking.time"
        required
        placeholder=""
      />
    </div>
    <template #footer>
      <Button
        @click="
          editBooking(booking);
          editDialog = false;
        "
        >Save</Button
      >
      <Button @click="editDialog = false">Cancel</Button>
    </template>
  </Dialog>

  <Dialog
    v-model:visible="deleteDialog"
    :style="{ width: '450px' }"
    header="Delete"
    :modal="true"
  >
    <div class="confirmation-content">
      <i class="pi pi-exclamation-triangle mr-3" style="font-size: 2rem" />
      <span v-if="booking">Are you sure you want to delete this booking?</span>
    </div>
    <template #footer>
      <Button
        label="No"
        icon="pi pi-times"
        class="p-button-text"
        @click="deleteDialog = false"
      />
      <Button
        label="Yes"
        icon="pi pi-check"
        class="p-button-text"
        @click="
          removeBooking(booking);
          deleteDialog = false;
        "
      />
    </template>
  </Dialog>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref, Ref } from 'vue';
import { Booking } from '@/model/booking';
import { useBookingsStore } from '@/stores/bookingsStore';
import { useHairdressersStore } from '@/stores/hairdressersStore';
import { useServicesStore } from '@/stores/servicesStore';
import { useSpecialOfferStore } from '@/stores/specialOfferStore';
import { storeToRefs } from 'pinia';

export default defineComponent({
  name: 'Bookings',

  setup() {
    const hairdressersStore = useHairdressersStore();
    const { hairdressers } = storeToRefs(hairdressersStore);

    const servicesStore = useServicesStore();
    const { services } = storeToRefs(servicesStore);

    const specialOfferStore = useSpecialOfferStore();
    const { specialOffers } = storeToRefs(specialOfferStore);

    const booking: Ref<Booking> = ref({
      id: 0,
      hairdresserId: 0,
      serviceId: 0,
      specialOfferId: 0,
      date: '',
      time: '',
    });
    const bookingsStore = useBookingsStore();
    const { bookings } = storeToRefs(bookingsStore);

    onMounted(() => {
      bookingsStore.load();
      hairdressersStore.load();
      servicesStore.load();
      specialOfferStore.load();
    });

    function createBooking() {
      bookingsStore.createBooking({ ...booking.value });
      booking.value.date = '';
      booking.value.time = '';
    }

    function removeBooking(booking: Booking) {
      bookingsStore.deleteBooking(booking);
    }

    function editBooking(editBooking: Booking) {
      bookingsStore.updateBooking(editBooking);
    }

    return {
      bookings,
      booking,
      createBooking,
      removeBooking,
      editBooking,
      hairdressers,
      services,
      specialOffers,
    };
  },
  data() {
    return {
      dialog: false,
      deleteDialog: false,
      editDialog: false,
      firstName: '',
      lastName: '',
      title: '',
      duration: '',
      code: '',
      percentage: '',
      price: '',
    };
  },
  methods: {
    openDialog() {
      this.dialog = true;
    },
    hideDialog() {
      this.dialog = false;
      this.editDialog = false;
      this.booking.date = '';
      this.booking.time = '';
    },
    confirmDeleteDialog(deleteBooking: Booking) {
      this.booking = { ...deleteBooking };
      this.deleteDialog = true;
    },
    editBookings(editBooking: Booking) {
      this.booking = { ...editBooking };
      this.editDialog = true;
    },
    countTotalPrice(price: number, percentage: number) {
      var totalPrice = price - price * percentage * 0.01;
      return totalPrice;
    },
  },
});
</script>
