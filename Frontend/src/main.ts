import { createApp } from 'vue';
import App from './App.vue';
import 'virtual:windi.css';
import PrimeVue from 'primevue/config';
import 'primevue/resources/themes/saga-blue/theme.css'; //theme import 'primevue/resources/primevue.min.css'; //core css
import 'primeicons/primeicons.css'; //icons
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import { createPinia } from 'pinia';
import router from './router';
import Button from 'primevue/button';
import Paginator from 'primevue/paginator';
import Dialog from 'primevue/dialog';
import InputText from 'primevue/inputtext';
import InputNumber from 'primevue/inputnumber';
import Toolbar from 'primevue/toolbar';
import { setApiUrl } from './model/api';
import Dropdown from 'primevue/dropdown';

const getRuntimeConf = async () => {
  const runtimeConf = await fetch('/config/runtime-config.json');
  return await runtimeConf.json();
};

getRuntimeConf().then((json) => {
  setApiUrl(json.API_URL);

  let app = createApp(App);

  app.use(createPinia());
  app.use(PrimeVue);
  app.use(router);

  app.component('Dropdown', Dropdown);
  app.component('Button', Button);
  app.component('Paginator', Paginator);
  app.component('DataTable', DataTable);
  app.component('Column', Column);
  app.component('Dialog', Dialog);
  app.component('InputText', InputText);
  app.component('InputNumber', InputNumber);
  app.component('Toolbar', Toolbar);

  app.mount('#app');
});
