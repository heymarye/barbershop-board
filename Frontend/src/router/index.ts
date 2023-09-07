import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router';

import Clients from '@/components/Clients.vue';
import Hairdressers from '@/components/Hairdressers.vue';
import Services from '@/components/Services.vue';
import Bookings from '@/components/Bookings.vue';
import SpecialOffer from '@/components/SpecialOffer.vue';
import LoginVue from '@/components/LoginForm.vue';
import { useAuthStore } from '@/stores/authStore';

const routes: Array<RouteRecordRaw> = [
  {
    path: '/clients',
    name: 'Kliendid',
    component: Clients,
    meta: { requiresAuth: true },
  },
  {
    path: '/barbers',
    name: 'Juuksurid',
    component: Hairdressers,
    meta: { requiresAuth: true },
  },
  {
    path: '/services',
    name: 'Teenused',
    component: Services,
    meta: { requiresAuth: true },
  },
  {
    path: '/bookings',
    name: 'Broneeringud',
    component: Bookings,
    meta: { requiresAuth: true },
  },
  {
    path: '/discounts',
    name: 'Sooduspakkumised',
    component: SpecialOffer,
    meta: { requiresAuth: true },
  },
  {
    path: '/',
    name: 'Sign in',
    component: LoginVue,
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

router.beforeEach((to, from, next) => {
  const useAuth = useAuthStore();
  if (to.matched.some((record) => record.meta.requiresAuth)) {
    if (useAuth.isAuthenticated) {
      next();
      return;
    }
    next('/');
  } else {
    next();
  }
});

export default router;
