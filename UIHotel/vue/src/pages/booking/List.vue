<template>
    <v-card>
        <v-card-title>
            <h2 class="card-title mb-0">Booking List</h2>
        </v-card-title>

        <v-container fluid grid-list-md>
            <v-layout row>
                <v-flex lg2 md2 sm12 xs12>
                    <v-btn color="primary" href="http://localhost.com/checkin/get/booking" block>Booking <v-icon dark right>bookmark</v-icon></v-btn>
                </v-flex>
                <v-flex lg10 md10 sm12 xs12>
                    <v-text-field placeholder="Search"></v-text-field>
                </v-flex>
            </v-layout>
            <v-layout row>
                <v-flex lg12>
                    <v-data-table v-bind:headers="tableData.headers"
                                  v-bind:items="tableData.items"
                                  v-bind:search="tableData.search"
                                  v-bind:pagination.sync="tableData.pagination"
                                  v-bind:total-items="tableData.totalItems"
                                  v-bind:loading="tableData.loading"
                                  class="elevation-1">
                        <template slot="items" slot-scope="props">
                            <tr :class="classColor(props.item.LateLevel)">
                                <td>{{ props.item.Id }}</td>
                                <td>
                                    <v-chip v-for="room in props.item.Rooms" :key="room.Id">
                                        <strong>{{ room.RoomNumber }}</strong>&nbsp;
                                        <span>({{ room.RoomCategory }})</span>
                                    </v-chip>
                                </td>
                                <td>{{ props.item.Guest.Fullname }}</td>
                                <td>{{ props.item.ArriveAt | dateformat }}</td>
                                <td>{{ props.item.DepartureAt | dateformat }}</td>
                                <td>
                                    <v-btn icon class="mx-0" :href="props.item.EditLink">
                                        <v-icon color="warning">create</v-icon>
                                    </v-btn>
                                    <v-btn icon class="mx-0" :href="props.item.CheckinLink">
                                        <v-icon color="success">assignment_turned_in</v-icon>
                                    </v-btn>
                                    <v-btn icon class="mx-0" @click.stop="confirmCancel(props.item)">
                                        <v-icon color="error">clear</v-icon>
                                    </v-btn>
                                </td>
                            </tr>
                        </template>
                        <template slot="pageText" slot-scope="{ pageStart, pageStop }">
                            From {{ pageStart }} to {{ pageStop }}
                        </template>
                    </v-data-table>
                    <confirm></confirm>
                    <alert></alert>
                </v-flex>
            </v-layout>
        </v-container>
    </v-card>
</template>
<script>
    import axios from 'axios'
    import moment from 'moment'
    import confirm from '../../components/dialog/ConfirmDialog.vue'
    import alert from '../../components/Notification.vue'
    export default {
        components: {
            'confirm': confirm,
            'alert': alert,
        },
        data() {
            return {
                tableData: {
                    search: '',
                    totalItems: 0,
                    loading: false,
                    pagination: {},
                    headers: [
                        { text: 'ID Booking', sortable: false, align: 'left' },
                        { text: 'Room Number', sortable: false, align: 'left' },
                        { text: 'Guest', sortable: false, align: 'left' },
                        { text: 'Arrival Date', sortable: false, align: 'left' },
                        { text: 'Departure Date', sortable: false, align: 'left' },
                        { text: 'Actions', sortable: false },
                    ],
                    items: []
                }
            }
        },
        filters: {
            dateformat(val) {
                var momen = moment(val)

                return momen.format('DD/MM/YYYY');
            }
        },
        methods: {
            confirmCancel(item) {
                let cancel = this.cancelBook

                this.$bus.$emit('dialog-show', {
                    title: 'Cancel Booking',
                    message: 'Are you sure to cancel this booking?',
                    onYes() { cancel(item) },
                    onNo() { }
                })
            },
            cancelBook(item) {
                axios.post(item.RemoveLink).then(this.cancelBookCallback);
            },
            cancelBookCallback(response) {
                let data = response.data

                if (data.success) {
                    this.getApi()
                    this.$bus.$emit('alert-show', { text: 'Success Delete', color: 'success' })
                } else {
                    this.$bus.$emit('alert-show', { text: 'Failed Delete', color: 'error' })
                }
            },
            classColor(level) {
                if (level == 1)
                    return ['red', 'lighten-5']
                else if (level == 0)
                    return ['yellow', 'lighten-5']
                else
                    return []
            },
            getApi() {
                const { page, rowsPerPage } = this.tableData.pagination
                const search = this.tableData.search
                this.tableData.loading = true

                axios.post('http://localhost.com/checkin/post/getBookingList', { page, rowsPerPage, search })
                    .then(this.getApiData)
                    .catch(() => { })
            },
            getApiData(response) {
                let data = response.data

                if (data.success) {
                    this.tableData.items = []
                    this.tableData.totalItems = data.total

                    data.data.forEach((x) => {
                        this.tableData.items.push(x)
                    })
                }

                this.tableData.loading = false
            }
        },
        mounted() {
            this.getApi()
        }
    }
</script>
