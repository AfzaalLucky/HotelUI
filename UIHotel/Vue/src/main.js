// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import '../../node_modules/vuetify/dist/vuetify.min.css'
import Vue from 'vue'
import Vuetify from 'vuetify'
import router from './router'
import Login from './pages/Login'
import Booking from './pages/booking/Booking'
import BookingList from './pages/booking/List'
import Checkin from './pages/checkin/Checkin'
import CheckinList from './pages/checkin/List'
import CheckinDetail from './pages/checkin/Detail'
import Checkout from './pages/checkin/Checkout'
import InvoiceDetail from './pages/guest/Invoice'
import InvoicePay from './pages/guest/Pay'
import GuestCreate from './pages/guest/Create'
import GuestList from './pages/guest/List'
import GuestEdit from './pages/guest/Edit'
import GuestDetail from './pages/guest/Detail'
import RoomDetail from './pages/room/Detail'
import RoomCategory from './pages/room/Category'
import RoomCalendar from './pages/room/Calendar'
import RoomPrice from './pages/room/Price'
import RoomList from './pages/room/List'
import RoomMaintain from './pages/room/Setting'
import SettingBasic from './pages/settings/Basic'
import SettingUser from './pages/settings/User'

Vue.use(Vuetify)
Vue.config.productionTip = false
Vue.component('login', Login)
Vue.component('booking', Booking)
Vue.component('booking-list', BookingList)
Vue.component('checkin', Checkin)
Vue.component('checkin-list', CheckinList)
Vue.component('checkin-detail', CheckinDetail)
Vue.component('checkout', Checkout)
Vue.component('invoice-detail', InvoiceDetail)
Vue.component('invoice-pay', InvoicePay)
Vue.component('guest-create', GuestCreate)
Vue.component('guest-list', GuestList)
Vue.component('guest-edit', GuestEdit)
Vue.component('guest-detail', GuestDetail)
Vue.component('room-detail', RoomDetail)
Vue.component('room-category', RoomCategory)
Vue.component('room-calendar', RoomCalendar)
Vue.component('room-list', RoomList)
Vue.component('room-price', RoomPrice)
Vue.component("room-setting", RoomMaintain)
Vue.component('setting-basic', SettingBasic)
Vue.component('setting-user', SettingUser)

/* eslint-disable no-new */
Vue.prototype.$bus = new Vue()
new Vue({
  el: '#app',
  //router,
  //template: '<App/>',
  //components: { App }
})
