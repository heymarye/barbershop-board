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
      :value="services"
      :paginator="true"
      :rows="10"
      :rowsPerPageOptions="[10, 20, 50]"
      responsiveLayout="scroll"
      paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown"
      currentPageReportTemplate="Showing {first} to {last} of {totalRecords} services"
    >
      <Column header="Label" field="title" :sortable="true"></Column>
      <Column header="Description" field="description"></Column>
      <Column header="Price" field="price" :sortable="true">
        <template #body="slotProps">
          <div>{{ slotProps.data.price }}€</div>
        </template></Column
      >
      <Column header="Duration" field="duration"></Column>
      <Column :exportable="false">
        <template #body="id">
          <Button
            icon="pi pi-pencil"
            class="p-button-rounded p-button-success p-button-text mr-2"
            @click="editServices(id.data)"
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
      <label for="title">Label</label>
      <input
        id="title"
        class="inputfield w-full"
        type="text"
        style="border: 0.5px solid; height: 40px; margin-bottom: 10px"
        v-model="service.title"
        required
        placeholder=""
      />
    </div>
    <div class="field">
      <label for="description">Description</label>
      <input
        id="description"
        class="inputfield w-full"
        type="text"
        style="border: 0.5px solid; height: 40px; margin-bottom: 10px"
        v-model="service.description"
        required
        placeholder=""
      />
    </div>
    <div class="field">
      <label for="price">Price</label>
      <input
        id="price"
        class="inputfield w-full"
        type="number"
        style="border: 0.5px solid; height: 40px; margin-bottom: 10px"
        v-model="service.price"
        required
        placeholder=""
      />
    </div>
    <div class="field">
      <label for="duration">Duration</label>
      <input
        id="duration"
        class="inputfield w-full"
        type="time"
        style="border: 0.5px solid; height: 40px; margin-bottom: 10px"
        v-model="service.duration"
        required
        placeholder=""
      />
    </div>
    <template #footer>
      <Button
        @click="
          createService();
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
      <label for="title">Label</label>
      <input
        id="title"
        class="inputfield w-full"
        type="text"
        style="border: 0.5px solid; height: 40px; margin-bottom: 10px"
        v-model="service.title"
        required
        placeholder=""
      />
    </div>
    <div class="field">
      <label for="description">Description</label>
      <input
        id="description"
        class="inputfield w-full"
        type="text"
        style="border: 0.5px solid; height: 40px; margin-bottom: 10px"
        v-model="service.description"
        required
        placeholder=""
      />
    </div>
    <div class="field">
      <label for="price">Price</label>
      <input
        id="price"
        class="inputfield w-full"
        type="number"
        style="border: 0.5px solid; height: 40px; margin-bottom: 10px"
        v-model="service.price"
        required
        placeholder=""
      />
    </div>
    <div class="field">
      <label for="duration">Duration</label>
      <input
        id="duration"
        class="inputfield w-full"
        type="time"
        style="border: 0.5px solid; height: 40px; margin-bottom: 10px"
        v-model="service.duration"
        required
        placeholder=""
      />
    </div>
    <template #footer>
      <Button
        @click="
          editService(service);
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
      <span v-if="service"
        >Are you sure you want to delete <b>{{ service.title }}</b> from
        services?</span
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
          removeService(service);
          deleteDialog = false;
        "
      />
    </template>
  </Dialog>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref, Ref } from 'vue';
import { Service } from '@/model/service';
import { useServicesStore } from '@/stores/servicesStore';
import { storeToRefs } from 'pinia';

export default defineComponent({
  name: 'Services',

  setup() {
    const service: Ref<Service> = ref({
      id: 0,
      title: '',
      description: '',
      price: 0,
      duration: '',
    });
    const servicesStore = useServicesStore();
    const { services } = storeToRefs(servicesStore);

    onMounted(() => {
      servicesStore.load();
    });

    function createService() {
      servicesStore.createNewService({ ...service.value });
      service.value.title = '';
      service.value.description = '';
      service.value.price = 0;
      service.value.duration = '';
    }

    function removeService(service: Service) {
      servicesStore.deleteService(service);
    }

    function editService(editService: Service) {
      servicesStore.updateService(editService);
    }

    return {
      services,
      service,
      createService,
      removeService,
      editService,
    };
  },
  data() {
    return {
      dialog: false,
      deleteDialog: false,
      editDialog: false,
    };
  },
  methods: {
    openDialog() {
      this.dialog = true;
    },
    hideDialog() {
      this.dialog = false;
      this.service.title = '';
      this.service.description = '';
      this.service.price = 0;
      this.service.duration = '';
    },
    confirmDeleteDialog(deleteService: Service) {
      this.service = { ...deleteService };
      this.deleteDialog = true;
    },
    editServices(editService: Service) {
      this.service = { ...editService };
      this.editDialog = true;
    },
  },
});
</script>
