import Vue from "vue";
import Vuelidate from "vee-validate";
import * as uiv from "uiv";
import App from "./App.vue";
import router from "./router";
import store from "./store/index";
import { Register } from "@/lib/List/Filter";
import "./registerGlobalScope";
import "./registerServiceWorker";

Register();
Vue.use(uiv, { prefix: "uiv" });
Vue.use(Vuelidate);
Vue.config.productionTip = false;

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount("#page-container");

import "@/App.css";