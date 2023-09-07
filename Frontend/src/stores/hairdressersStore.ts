import { Hairdresser } from '@/model/hairdresser';
import { defineStore } from 'pinia';
import { ref } from 'vue';
import useApi, { useApiRawRequest } from '@/model/api';
import HairdressersVue from '@/components/Hairdressers.vue';
import { useAuthStore } from './authStore';

export const useHairdressersStore = defineStore('hairdressersStore', () => {
  const authStore = useAuthStore();
  let allHairdressers: Hairdresser[] = [];
  let hairdressers = ref<Hairdresser[]>([]);

  const loadHairdressers = async () => {
    const apiHairdressers = useApi<Hairdresser[]>('Hairdresser', {
      headers: { Authorization: 'Bearer ' + authStore.token },
    });
    await apiHairdressers.request();
    if (apiHairdressers.response.value) {
      return apiHairdressers.response.value!;
    }
    return [];
  };

  const load = async () => {
    allHairdressers = await loadHairdressers();
    hairdressers.value = allHairdressers;
  };

  const createHairdresser = async (hairdresser: Hairdresser) => {
    const apiAddHaidresser = useApi<Hairdresser>('Hairdresser', {
      method: 'POST',
      headers: {
        Authorization: 'Bearer ' + authStore.token,
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(hairdresser),
    });

    await apiAddHaidresser.request();

    if (apiAddHaidresser.response.value) {
      allHairdressers.push(apiAddHaidresser.response.value!);
      hairdressers.value = allHairdressers;
    }
  };

  const updateHairdresser = async (hairdresser: Hairdresser) => {
    const apiUpdateHairdresser = useApi<Hairdresser>(
      `Hairdresser/${hairdresser.id}`,
      {
        method: 'PUT',
        headers: {
          Authorization: 'Bearer ' + authStore.token,
          Accept: 'application/json',
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(hairdresser),
      },
    );

    try {
      await apiUpdateHairdresser.request();
    } catch (error) {}

    const index = findByIndex(hairdresser.id);
    hairdressers.value[index] = hairdresser;
  };

  const deleteHairdresser = async (hairdresser: Hairdresser) => {
    const deleteHaidresserRequest = useApiRawRequest(
      `Hairdresser/${hairdresser.id}`,
      {
        method: 'DELETE',
        headers: { Authorization: 'Bearer ' + authStore.token },
      },
    );

    const res = await deleteHaidresserRequest();

    if (res.status === 204) {
      let id = allHairdressers.indexOf(hairdresser);

      if (id !== -1) {
        allHairdressers.splice(id, 1);
      }
    }
    const index = findByIndex(hairdresser.id);

    if (index !== -1) {
      hairdressers.value.splice(index, 1);
    }
  };

  const findByIndex = (id: number) => {
    return hairdressers.value.findIndex((hairdresser) => hairdresser.id === id);
  };

  return {
    hairdressers,
    createHairdresser,
    updateHairdresser,
    deleteHairdresser,
    load,
  };
});
