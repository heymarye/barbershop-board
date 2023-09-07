<template>
  <nav class="bg-yellow-400">
    <div class="max-w-7xl mx-auto px-2 sm:px-6 lg:px-8">
      <div class="relative flex items-center justify-between h-16">
        <div class="flex-1 flex items-center justify-center sm:items-stretch sm:justify-start">
          <div class="hidden sm:block sm:ml-6">
            <div class="flex space-x-4">
              <router-link
                to="/clients"
                class="text-black-50 hover:bg-yellow-500 hover:text-white px-3 py-2 rounded-md text-sm font-medium"
                active-class="bg-gray-50 text-black"
                aria-current="page"
              >Clients</router-link>
              <router-link
                to="/barbers"
                class="text-black-50 hover:bg-yellow-500 hover:text-white px-3 py-2 rounded-md text-sm font-medium"
                active-class="bg-gray-50 text-black"
                aria-current="page"
              >Barbers</router-link>
              <router-link
                to="/services"
                class="text-black-50 hover:bg-yellow-500 hover:text-white px-3 py-2 rounded-md text-sm font-medium"
                active-class="bg-gray-50 text-black"
                aria-current="page"
              >Services</router-link>
              <router-link
                to="/bookings"
                class="text-black-50 hover:bg-yellow-500 hover:text-white px-3 py-2 rounded-md text-sm font-medium"
                active-class="bg-gray-50 text-black"
                aria-current="page"
              >Bookings</router-link>
              <router-link
                to="/discounts"
                class="text-black-50 hover:bg-yellow-500 hover:text-white px-3 py-2 rounded-md text-sm font-medium"
                active-class="bg-gray-50 text-black"
                aria-current="page"
              >Discounts</router-link>
            </div>
          </div>
          </div>
          <div
          class="absolute inset-y-0 right-0 flex items-center pr-2 sm:static sm:inset-auto sm:ml-6 sm:pr-0 ml-3 relative"
        >
          <button
            type="button"
            v-if="isAuthenticated"
            class="text-white font-medium flex text-sm rounded-full focus:outline-none"
            @click="signOut"
          >
            Sign out
          </button>
        </div>
        </div>
    </div>
  </nav>
  <router-view />
</template>

<script setup lang="ts">
import { storeToRefs } from 'pinia';
import router from './router';
import { useAuthStore } from './stores/authStore';

const authStore = useAuthStore();
const { logout } = authStore;
const { isAuthenticated } = storeToRefs(authStore);

const signOut = () => {
  logout();
  router.push({ name: 'Sign in' });
};
</script>