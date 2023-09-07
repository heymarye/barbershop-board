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
      :value="specialOffers"
      :paginator="true"
      :rows="10"
      :rowsPerPageOptions="[10, 20, 50]"
      responsiveLayout="scroll"
      paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown"
      currentPageReportTemplate="Showing {first} to {last} of {totalRecords} services"
    >
      <Column header="Code" field="code"></Column>
      <Column header="Discount (%)" field="percentage">
        <template #body="slotProps">
          <div>{{ slotProps.data.percentage }}%</div>
        </template>
      </Column>
      <Column header="Service" field="serviceId" :sortable="true">
        <template #body="slotProps">
          <div>
            {{ services.at(slotProps.data.serviceId - 1)?.title }}
          </div>
        </template>
      </Column>
      <Column header="From" field="fromDate" :sortable="true"></Column>
      <Column header="To" field="toDate" :sortable="true">
        <template #body="slotProps">
          <div :style="{ color: isWarning(slotProps.data) ? 'black' : 'red' }">
            {{ slotProps.data.toDate }}
          </div>
        </template>
      </Column>
      <Column :exportable="false">
        <template #body="id">
          <Button
            icon="pi pi-pencil"
            class="p-button-rounded p-button-success p-button-text mr-2"
            @click="editSpecialOffer(id.data)"
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
      <label for="percentage">Discount</label>
      <input
        id="percentage"
        class="inputfield w-full"
        type="number"
        style="border: 0.5px solid; height: 40px; margin-bottom: 10px"
        v-model="specialOffer.percentage"
        required
        placeholder=""
      />
    </div>
    <div class="field">
      <label for="service">Service</label>
      <br />
      <select
        v-model="specialOffer.serviceId"
        style="border: 0.5px solid; height: 40px; margin-bottom: 10px"
      >
        <option v-for="option in services" :value="option.id">
          {{ option.title }}
        </option>
      </select>
    </div>
    <div class="field">
      <label for="fromDate">From</label>
      <input
        id="fromDate"
        class="inputfield w-full"
        type="date"
        style="border: 0.5px solid; height: 40px; margin-bottom: 10px"
        v-model="specialOffer.fromDate"
        required
        placeholder=""
      />
    </div>
    <div class="field">
      <label for="toDate">To</label>
      <input
        id="toDate"
        class="inputfield w-full"
        type="date"
        style="border: 0.5px solid; height: 40px; margin-bottom: 10px"
        v-model="specialOffer.toDate"
        required
        placeholder=""
      />
    </div>
    <template #footer>
      <Button
        @click="
          createSpecialOffer();
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
      <label for="percentage">Discount</label>
      <input
        id="percentage"
        class="inputfield w-full"
        type="number"
        style="border: 0.5px solid; height: 40px; margin-bottom: 10px"
        v-model="specialOffer.percentage"
        required
        placeholder=""
      />
    </div>
    <div class="field">
      <label for="service">Service</label>
      <br />
      <select
        v-model="specialOffer.serviceId"
        style="border: 0.5px solid; height: 40px; margin-bottom: 10px"
      >
        <option v-for="option in services" :value="option.id">
          {{ option.title }}
        </option>
      </select>
    </div>
    <div class="field">
      <label for="fromDate">From</label>
      <input
        id="fromDate"
        class="inputfield w-full"
        type="date"
        style="border: 0.5px solid; height: 40px; margin-bottom: 10px"
        v-model="specialOffer.fromDate"
        required
        placeholder=""
      />
    </div>
    <div class="field">
      <label for="toDate">To</label>
      <input
        id="toDate"
        class="inputfield w-full warning"
        type="date"
        style="border: 0.5px solid; height: 40px; margin-bottom: 10px"
        v-model="specialOffer.toDate"
        required
        placeholder=""
      />
    </div>
    <template #footer>
      <Button
        @click="
          editSpecialOffers(specialOffer);
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
      <span v-if="specialOffer"
        >Are you sure you want to delete this discount?</span
      >
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
          removeSpecialOffer(specialOffer);
          deleteDialog = false;
        "
      />
    </template>
  </Dialog>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref, Ref } from 'vue';
import { SpecialOffer } from '@/model/specialOffer';
import { useSpecialOfferStore } from '@/stores/specialOfferStore';
import { useServicesStore } from '@/stores/servicesStore';
import { storeToRefs } from 'pinia';

export default defineComponent({
  name: 'SpecialOffers',

  setup() {
    const servicesStore = useServicesStore();
    const { services } = storeToRefs(servicesStore);

    const specialOffer: Ref<SpecialOffer> = ref({
      id: 0,
      serviceId: 0,
      code: '',
      percentage: 0,
      fromDate: '',
      toDate: '',
    });
    const specialOfferStore = useSpecialOfferStore();
    const { specialOffers } = storeToRefs(specialOfferStore);

    onMounted(() => {
      specialOfferStore.load();
      servicesStore.load();
    });

    function createSpecialOffer() {
      specialOfferStore.createNewSpecialOffer({ ...specialOffer.value });
      specialOffer.value.percentage = 0;
      specialOffer.value.fromDate = '';
      specialOffer.value.toDate = '';
    }

    function removeSpecialOffer(specialOffer: SpecialOffer) {
      specialOfferStore.deleteSpecialOffer(specialOffer);
    }

    function editSpecialOffers(editSpecialOffer: SpecialOffer) {
      specialOfferStore.updateSpecialOffer(editSpecialOffer);
    }

    const isWarning = (specialOffer: SpecialOffer) => {
      let today = new Date();
      let date;
      let month = today.getMonth() + 1;
      let year = today.getFullYear();
      let day = today.getDate();
      if (today.getMonth() < 10) {
        date = year + '-0' + month + '-' + day;
      } else {
        date = year + '-' + month + '-' + day;
      }
      if (specialOffer.toDate > date) {
        return true;
      } else {
        return false;
      }
    };

    return {
      specialOffers,
      specialOffer,
      isWarning,
      createSpecialOffer,
      removeSpecialOffer,
      editSpecialOffers,
      services,
    };
  },
  data() {
    return {
      dialog: false,
      deleteDialog: false,
      editDialog: false,
      title: '',
    };
  },
  methods: {
    openDialog() {
      this.dialog = true;
    },
    hideDialog() {
      this.dialog = false;
      this.editDialog = false;
      this.specialOffer.percentage = 0;
      this.specialOffer.toDate = '';
      this.specialOffer.fromDate = '';
    },
    confirmDeleteDialog(deleteSpecialOffer: SpecialOffer) {
      this.specialOffer = { ...deleteSpecialOffer };
      this.deleteDialog = true;
    },
    editSpecialOffer(editSpecialOffer: SpecialOffer) {
      this.specialOffer = { ...editSpecialOffer };
      this.editDialog = true;
    },
  },
});
</script>
