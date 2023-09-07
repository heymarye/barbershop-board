<template>
  <div class="py-5 px-8">
    <Toolbar class="mb-4">
      <template #start>
        <Button
          label="Add"
          icon="pi pi-plus"
          class="p-button-success mr-2"
          @click="openDialog"
        />
      </template>
    </Toolbar>
    <DataTable
      :value="hairdressers"
      :paginator="true"
      :rows="10"
      :rowsPerPageOptions="[10, 20, 50]"
      responsiveLayout="scroll"
      paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown"
      currentPageReportTemplate="Showing {first} to {last} of {totalRecords} hairdressers"
    >
      <Column header="Name" field="firstName" :sortable="true"></Column>
      <Column header="Surname" field="lastName" :sortable="true"></Column>
      <Column header="Email" field="mail"></Column>
      <Column header="Phone" field="mobile"></Column>
      <Column :exportable="false">
        <template #body="id">
          <Button
            icon="pi pi-pencil"
            class="p-button-rounded p-button-success p-button-text mr-2"
            @click="updateHairdressers(id.data)"
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
      <label for="firstName">Name</label>
      <input
        id="firstName"
        class="inputfield w-full"
        type="text"
        style="border: 0.5px solid; height: 40px; margin-bottom: 10px"
        v-model="hairdresser.firstName"
        required
        placeholder=""
      />
    </div>
    <div class="field">
      <label for="lastName">Surname</label>
      <input
        id="lastName"
        class="inputfield w-full"
        type="text"
        style="border: 0.5px solid; height: 40px; margin-bottom: 10px"
        v-model="hairdresser.lastName"
        required
        placeholder=""
      />
    </div>
    <div class="field">
      <label for="mail">Email</label>
      <input
        id="mail"
        class="inputfield w-full"
        type="mail"
        style="border: 0.5px solid; height: 40px; margin-bottom: 10px"
        v-model="hairdresser.mail"
        required
        placeholder=""
      />
    </div>
    <div class="field">
      <label for="mobile">Phone</label>
      <input
        id="mobile"
        class="inputfield w-full"
        type="string"
        style="border: 0.5px solid; height: 40px; margin-bottom: 10px"
        v-model="hairdresser.mobile"
        required
        placeholder=""
      />
    </div>
    <template #footer>
      <Button
        @click="
          createHairdresser();
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
      <label for="firstName">Name</label>
      <input
        id="firstName"
        class="inputfield w-full"
        type="text"
        style="border: 0.5px solid; height: 40px; margin-bottom: 10px"
        v-model="hairdresser.firstName"
        required
        placeholder=""
      />
    </div>
    <div class="field">
      <label for="lastName">Surname</label>
      <input
        id="lastName"
        class="inputfield w-full"
        type="text"
        style="border: 0.5px solid; height: 40px; margin-bottom: 10px"
        v-model="hairdresser.lastName"
        required
        placeholder=""
      />
    </div>
    <div class="field">
      <label for="mail">Email</label>
      <input
        id="mail"
        class="inputfield w-full"
        type="email"
        style="border: 0.5px solid; height: 40px; margin-bottom: 10px"
        v-model="hairdresser.mail"
        required
        placeholder=""
      />
    </div>
    <div class="field">
      <label for="mobile">Phone</label>
      <input
        id="mobile"
        class="inputfield w-full"
        type="string"
        style="border: 0.5px solid; height: 40px; margin-bottom: 10px"
        v-model="hairdresser.mobile"
        required
        placeholder=""
      />
    </div>
    <template #footer>
      <Button
        @click="
          updateHairdresser(hairdresser);
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
      <span v-if="hairdresser"
        >Are you sure you want to delete
        <b>{{ hairdresser.firstName }} {{ hairdresser.lastName }}</b> from
        barbers?</span
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
          deleteHairdresser(hairdresser);
          deleteDialog = false;
        "
      />
    </template>
  </Dialog>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref, Ref } from 'vue';
import { Hairdresser } from '@/model/hairdresser';
import { useHairdressersStore } from '@/stores/hairdressersStore';
import { storeToRefs } from 'pinia';

export default defineComponent({
  name: 'Hairdressers',

  setup() {
    const hairdresser: Ref<Hairdresser> = ref({
      id: 0,
      firstName: '',
      lastName: '',
      mail: '',
      mobile: '',
    });
    const hairdressersStore = useHairdressersStore();
    const { hairdressers } = storeToRefs(hairdressersStore);

    onMounted(() => {
      //hairdressers.value = hairdressersStore.hairdressers;
      hairdressersStore.load();
    });

    function createHairdresser() {
      //hairdressersStore.createHairdresser(hairdresser.value);
      hairdressersStore.createHairdresser({ ...hairdresser.value });
      hairdresser.value.firstName = '';
      hairdresser.value.lastName = '';
      hairdresser.value.mail = '';
      hairdresser.value.mobile = '';
    }

    function updateHairdresser(updateHairdresser: Hairdresser) {
      hairdressersStore.updateHairdresser(updateHairdresser);
    }

    function deleteHairdresser(hairdresser: Hairdresser) {
      hairdressersStore.deleteHairdresser(hairdresser);
    }

    return {
      hairdressers,
      hairdresser,
      createHairdresser,
      updateHairdresser,
      deleteHairdresser,
    };
  },
  data() {
    return {
      dialog: false,
      editDialog: false,
      deleteDialog: false,
    };
  },
  methods: {
    openDialog() {
      this.dialog = true;
    },
    hideDialog() {
      this.dialog = false;
      this.hairdresser.firstName = '';
      this.hairdresser.lastName = '';
      this.hairdresser.mail = '';
      this.hairdresser.mobile = '';
    },
    updateHairdressers(updateHairdresser: Hairdresser) {
      this.hairdresser = { ...updateHairdresser };
      this.editDialog = true;
    },
    confirmDeleteDialog(deleteHairdresser: Hairdresser) {
      this.hairdresser = { ...deleteHairdresser };
      this.deleteDialog = true;
    },
  },
});
</script>
