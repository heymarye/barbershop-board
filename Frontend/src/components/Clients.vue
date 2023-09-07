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
      :value="clients"
      :paginator="true"
      :rows="10"
      :rowsPerPageOptions="[10, 20, 50]"
      responsiveLayout="scroll"
      paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown"
      currentPageReportTemplate="Showing {first} to {last} of {totalRecords} clients"
    >
      <Column header="Name" field="firstName" :sortable="true"></Column>
      <Column header="Surname" field="lastName" :sortable="true"></Column>
      <Column header="Address" field="address"></Column>
      <Column header="Email" field="mail"></Column>
      <Column header="Phone" field="mobile"></Column>
      <Column :exportable="false">
        <template #body="id">
          <Button
            icon="pi pi-pencil"
            class="p-button-rounded p-button-success p-button-text mr-2"
            @click="editClients(id.data)"
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
        v-model="client.firstName"
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
        v-model="client.lastName"
        required
        placeholder=""
      />
    </div>
    <div class="field">
      <label for="address">Address</label>
      <input
        id="address"
        class="inputfield w-full"
        type="text"
        style="border: 0.5px solid; height: 40px; margin-bottom: 10px"
        v-model="client.address"
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
        v-model="client.mail"
        required
        placeholder=""
      />
    </div>
    <div class="field">
      <label for="mobile">Phone</label>
      <input
        id="mobile"
        class="inputfield w-full"
        type="text"
        style="border: 0.5px solid; height: 40px; margin-bottom: 10px"
        v-model="client.mobile"
        required
        placeholder=""
      />
    </div>
    <template #footer>
      <Button
        @click="
          createClient();
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
        v-model="client.firstName"
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
        v-model="client.lastName"
        required
        placeholder=""
      />
    </div>
    <div class="field">
      <label for="address">Address</label>
      <input
        id="address"
        class="inputfield w-full"
        type="text"
        style="border: 0.5px solid; height: 40px; margin-bottom: 10px"
        v-model="client.address"
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
        v-model="client.mail"
        required
        placeholder=""
      />
    </div>
    <div class="field">
      <label for="mobile">Phone</label>
      <input
        id="mobile"
        class="inputfield w-full"
        type="text"
        style="border: 0.5px solid; height: 40px; margin-bottom: 10px"
        v-model="client.mobile"
        required
        placeholder=""
      />
    </div>
    <template #footer>
      <Button
        @click="
          editClient(client);
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
      <span v-if="client"
        >Are you sure you want to delete
        <b>{{ client.firstName }} {{ client.lastName }}</b> from clients?</span
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
          removeClient(client);
          deleteDialog = false;
        "
      />
    </template>
  </Dialog>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref, Ref } from 'vue';
import { Client } from '@/model/client';
import { useClientsStore } from '@/stores/clientsStore';
import { storeToRefs } from 'pinia';
import useVuelidate from '@vuelidate/core';
import { required, email } from '@vuelidate/validators';
import { reactive } from 'vue';
import { validate } from '@babel/types';

export default defineComponent({
  name: 'Clients',

  setup() {
    const client: Ref<Client> = ref({
      id: 0,
      firstName: '',
      lastName: '',
      address: '',
      mail: '',
      mobile: '',
    });
    const clientsStore = useClientsStore();
    const { clients } = storeToRefs(clientsStore);

    onMounted(() => {
      clientsStore.load();
    });

    function createClient() {
      clientsStore.createNewClient({ ...client.value });
      client.value.firstName = '';
      client.value.lastName = '';
      client.value.address = '';
      client.value.mail = '';
      client.value.mobile = '';
    }

    function removeClient(client: Client) {
      clientsStore.deleteClient(client);
    }

    function editClient(editClient: Client) {
      clientsStore.updateClient(editClient);
    }

    return {
      clients,
      client,
      createClient,
      removeClient,
      editClient,
      v$: useVuelidate(),
    };
  },
  data() {
    return {
      dialog: false,
      deleteDialog: false,
      editDialog: false,
    };
  },
  validations() {
    return {
      client: { required },
    };
  },
  methods: {
    openDialog() {
      this.dialog = true;
    },
    hideDialog() {
      this.dialog = false;
      this.client.firstName = '';
      this.client.lastName = '';
      this.client.address = '';
      this.client.mail = '';
      this.client.mobile = '';
    },
    confirmDeleteDialog(deleteClient: Client) {
      this.client = { ...deleteClient };
      this.deleteDialog = true;
    },
    editClients(editClient: Client) {
      this.client = { ...editClient };
      this.editDialog = true;
    },
    onSubmit() {
      console.log('submitted');
    },
    validateEmail(value: string) {
      // if the field is empty
      if (!value) {
        return 'This field is required';
      }
      // if the field is not a valid email
      const regex = /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$/i;
      if (!regex.test(value)) {
        return 'This field must be a valid email';
      }
      // All is good
      return true;
    },
  },
});
</script>
