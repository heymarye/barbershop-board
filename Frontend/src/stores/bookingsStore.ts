import { Booking } from '@/model/booking';
import { defineStore } from 'pinia';
import { ref } from 'vue';
import useApi, { useApiRawRequest } from '@/model/api';
import { useAuthStore } from './authStore';

export const useBookingsStore = defineStore('bookingsStore', () => {
  const authStore = useAuthStore();
  let allBookings: Booking[] = [];
  let bookings = ref<Booking[]>([]);

  const createBooking = async (booking: Booking) => {
    const apiAddBooking = useApi<Booking>('Booking', {
      method: 'POST',
      headers: {
        Authorization: 'Bearer ' + authStore.token,
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(booking),
    });

    await apiAddBooking.request();

    if (apiAddBooking.response.value) {
      allBookings.push(apiAddBooking.response.value!);
      bookings.value = allBookings;
    }
  };

  const updateBooking = async (booking: Booking) => {
    const apiUpdateBooking = useApi<Booking>(`Booking/${booking.id}`, {
      method: 'PUT',
      headers: {
        Authorization: 'Bearer ' + authStore.token,
        Accept: 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(booking),
    });

    try {
      await apiUpdateBooking.request();
    } catch (error) {}

    const index = findByIndex(booking.id);
    bookings.value[index] = booking;
  };

  const deleteBooking = async (booking: Booking) => {
    const deleteBookingRequest = useApiRawRequest(`Booking/${booking.id}`, {
      method: 'DELETE',
      headers: { Authorization: 'Bearer ' + authStore.token },
    });

    const res = await deleteBookingRequest();

    if (res.status === 204) {
      let id = allBookings.indexOf(booking);

      if (id !== -1) {
        allBookings.splice(id, 1);
      }
    }
    const index = findByIndex(booking.id);

    if (index !== -1) {
      bookings.value.splice(index, 1);
    }
  };

  const findByIndex = (id: number) => {
    return bookings.value.findIndex((booking) => booking.id === id);
  };

  const loadBooking = async () => {
    const apiBookings = useApi<Booking[]>('Booking', {
      headers: { Authorization: 'Bearer ' + authStore.token },
    });

    await apiBookings.request();

    if (apiBookings.response.value) {
      return apiBookings.response.value!;
    }

    return [];
  };

  const load = async () => {
    allBookings = await loadBooking();
    bookings.value = allBookings;
  };

  return { bookings, createBooking, updateBooking, deleteBooking, load };
});
