import { Service } from '@/model/service';
import { defineStore } from 'pinia';
import { ref } from 'vue';
import useApi, { useApiRawRequest } from '@/model/api';
import { useAuthStore } from './authStore';

export const useServicesStore = defineStore('servicesStore', () => {
  const authStore = useAuthStore();
  let allServices: Service[] = [];
  let services = ref<Service[]>([]);

  const loadServices = async () => {
    const apiServices = useApi<Service[]>('Service', {
      headers: { Authorization: 'Bearer ' + authStore.token },
    });
    await apiServices.request();
    if (apiServices.response.value) {
      return apiServices.response.value!;
    }
    return [];
  };

  const load = async () => {
    allServices = await loadServices();
    services.value = allServices;
  };

  const createNewService = async (service: Service) => {
    const apiAddService = useApi<Service>('Service', {
      method: 'POST',
      headers: {
        Authorization: 'Bearer ' + authStore.token,
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(service),
    });

    await apiAddService.request();

    if (apiAddService.response.value) {
      if (service.price < 0) {
        return Error;
      } else {
        allServices.push(apiAddService.response.value!);
        services.value = allServices;
      }
    }
  };

  const updateService = async (service: Service) => {
    const apiUpdateService = useApi<Service>(`Service/${service.id}`, {
      method: 'PUT',
      headers: {
        Authorization: 'Bearer ' + authStore.token,
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(service),
    });

    try {
      await apiUpdateService.request();
    } catch (error) {}

    const index = findByIndex(service.id);
    services.value[index] = service;
  };

  const deleteService = async (service: Service) => {
    const deleteServiceRequest = useApiRawRequest(`Service/${service.id}`, {
      method: 'DELETE',
      headers: { Authorization: 'Bearer ' + authStore.token },
    });

    const res = await deleteServiceRequest();

    if (res.status === 204) {
      let id = allServices.indexOf(service);

      if (id !== -1) {
        allServices.splice(id, 1);
      }
    }
    const index = findByIndex(service.id);

    if (index !== -1) {
      services.value.splice(index, 1);
    }
  };

  const findByIndex = (id: number) => {
    return services.value.findIndex((service) => service.id === id);
  };

  return { services, createNewService, updateService, deleteService, load };
});
