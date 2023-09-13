import { SpecialOffer } from '@/model/specialOffer';
import { defineStore } from 'pinia';
import { ref } from 'vue';
import router from '@/router';
import useApi, { useApiRawRequest } from '@/model/api';
import { useAuthStore } from './authStore';

export const useSpecialOfferStore = defineStore('specialOfferStore', () => {
  const authStore = useAuthStore();
  let allSpecialOffers: SpecialOffer[] = [];
  let specialOffers = ref<SpecialOffer[]>([]);

  const loadSpecialOffers = async () => {
    const apiSpecialOffers = useApi<SpecialOffer[]>('SpecialOffer', {
      headers: { Authorization: 'Bearer ' + authStore.token },
    });
    await apiSpecialOffers.request();
    if (apiSpecialOffers.response.value) {
      return apiSpecialOffers.response.value!;
    }
    return [];
  };

  const load = async () => {
    allSpecialOffers = await loadSpecialOffers();
    specialOffers.value = allSpecialOffers;
  };

  const createNewSpecialOffer = async (specialOffer: SpecialOffer) => {
    const apiAddSpecialOffer = useApi<SpecialOffer>('SpecialOffer', {
      method: 'POST',
      headers: {
        Authorization: 'Bearer ' + authStore.token,
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(specialOffer),
    });

    await apiAddSpecialOffer.request();

    if (apiAddSpecialOffer.response.value) {
      allSpecialOffers.push(apiAddSpecialOffer.response.value!);
      specialOffers.value = allSpecialOffers;
    }
  };

  const updateSpecialOffer = async (specialOffer: SpecialOffer) => {
    const apiUpdateSpecialOffer = useApi<SpecialOffer>(
      `SpecialOffer/${specialOffer.id}`,
      {
        method: 'PUT',
        headers: {
          Authorization: 'Bearer ' + authStore.token,
          Accept: 'application/json',
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(specialOffer),
      },
    );

    try {
      await apiUpdateSpecialOffer.request();
    } catch (error) {}

    const index = findByIndex(specialOffer.id);
    specialOffers.value[index] = specialOffer;
  };

  const deleteSpecialOffer = async (specialOffer: SpecialOffer) => {
    const deleteSpecialOfferRequest = useApiRawRequest(
      `SpecialOffer/${specialOffer.id}`,
      {
        method: 'DELETE',
        headers: { Authorization: 'Bearer ' + authStore.token },
      },
    );

    const res = await deleteSpecialOfferRequest();

    if (res.status === 204) {
      let id = allSpecialOffers.indexOf(specialOffer);

      if (id !== -1) {
        allSpecialOffers.splice(id, 1);
      }

      id = allSpecialOffers.indexOf(specialOffer);

      if (id !== -1) {
        allSpecialOffers.splice(id, 1);
      }
    }
    const index = findByIndex(specialOffer.id);

    if (index !== -1) {
      specialOffers.value.splice(index, 1);
    }
  };

  const findByIndex = (id: number) => {
    return specialOffers.value.findIndex(
      (specialOffer) => specialOffer.id === id,
    );
  };

  return {
    specialOffers,
    createNewSpecialOffer,
    updateSpecialOffer,
    deleteSpecialOffer,
    load,
  };
});
