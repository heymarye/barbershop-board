import { Client } from '@/model/client';
import { defineStore } from 'pinia';
import { ref } from 'vue';
import useApi, { useApiRawRequest } from '@/model/api';
import { useAuthStore } from './authStore';

export const useClientsStore = defineStore('clientsStore', () => {
  const authStore = useAuthStore();
  let allClients: Client[] = [];
  let clients = ref<Client[]>([]);

  const loadClients = async () => {
    const apiClients = useApi<Client[]>('Client', {
      headers: { Authorization: 'Bearer ' + authStore.token },
    });
    await apiClients.request();
    if (apiClients.response.value) {
      return apiClients.response.value!;
    }
    return [];
  };

  const load = async () => {
    allClients = await loadClients();
    clients.value = allClients;
  };

  const createNewClient = async (client: Client) => {
    const apiAddClient = useApi<Client>('Client', {
      method: 'POST',
      headers: {
        Authorization: 'Bearer ' + authStore.token,
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(client),
    });

    await apiAddClient.request();

    if (apiAddClient.response.value) {
      allClients.push(apiAddClient.response.value!);
      clients.value = allClients;
    }
  };

  const updateClient = async (client: Client) => {
    const apiUpdateClient = useApi<Client>(`Client/${client.id}`, {
      method: 'PUT',
      headers: {
        Authorization: 'Bearer ' + authStore.token,
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(client),
    });

    try {
      await apiUpdateClient.request();
    } catch (error) {}

    const index = findByIndex(client.id);
    clients.value[index] = client;
  };

  const deleteClient = async (client: Client) => {
    const deleteClientRequest = useApiRawRequest(`Client/${client.id}`, {
      method: 'DELETE',
      headers: { Authorization: 'Bearer ' + authStore.token },
    });

    const res = await deleteClientRequest();

    if (res.status === 204) {
      let id = allClients.indexOf(client);

      if (id !== -1) {
        allClients.splice(id, 1);
      }
    }
    const index = findByIndex(client.id);

    if (index !== -1) {
      clients.value.splice(index, 1);
    }
  };

  const findByIndex = (id: number) => {
    return clients.value.findIndex((client) => client.id === id);
  };

  return { clients, createNewClient, updateClient, deleteClient, load };
});
