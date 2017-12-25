// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import '../../node_modules/vuetify/dist/vuetify.min.css'
import Vue from 'vue'
import Vuetify from 'vuetify'
import App from './App'
import SettingRoom from './pages/settings/Room'
import router from './router'

Vue.use(Vuetify)
Vue.config.productionTip = false
Vue.component("app", App)
Vue.component("setting-room", SettingRoom);

/* eslint-disable no-new */
new Vue({
  el: '#app',
  //router,
  //template: '<App/>',
  //components: { App }
})
