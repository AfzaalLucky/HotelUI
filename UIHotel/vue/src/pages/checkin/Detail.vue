<template>
    <div>
        <v-tabs color="blue-grey darken-1" class="no-print" slot="extension" v-model="tab" centered dark>
            <v-tabs-slider color="yellow"></v-tabs-slider>
            <v-tab>Detail</v-tab>
            <v-tab>Invoice</v-tab>
        </v-tabs>

        <v-tabs-items v-model="tab">
            <v-tab-item>
                <v-card>
                    <v-card-title>
                        <h2 class="card-title mb-0">Checkin Detail</h2>
                    </v-card-title>
                    <v-container fluid grid-list-md>
                        <v-layout row>
                            <v-flex lg3 md3 sm12 xs12>
                                <img style="max-height: 190px;" class="img-fluid img-thumbnail rounded" :src="PhotoUrl" @error="imageError" />
                            </v-flex>
                            <v-flex lg9 md9 sm12 xs12>
                                <v-layout row>
                                    <v-flex xs3>
                                        <v-subheader>Checkin Number</v-subheader>
                                    </v-flex>
                                    <v-flex xs4>
                                        <v-subheader>{{ checkin.Id }}</v-subheader>
                                    </v-flex>
                                </v-layout>
                                <v-layout row>
                                    <v-flex xs3>
                                        <v-subheader>Invoice Number</v-subheader>
                                    </v-flex>
                                    <v-flex xs4>
                                        <v-subheader>
                                            <a href="#" @click="tab = 1">{{ checkin.InvoiceId }}</a>
                                        </v-subheader>
                                    </v-flex>
                                </v-layout>
                                <v-layout row>
                                    <v-flex xs3>
                                        <v-subheader>Guest Name</v-subheader>
                                    </v-flex>
                                    <v-flex xs4>
                                        <v-subheader>
                                            <a :href="checkin.DetailGuest">{{ checkin.Fullname }}</a>
                                        </v-subheader>
                                    </v-flex>
                                </v-layout>
                                <v-layout row>
                                    <v-flex xs3>
                                        <v-subheader>Room Number</v-subheader>
                                    </v-flex>
                                    <v-flex xs4>
                                        <v-subheader>
                                            <a :href="checkin.DetailRoom">{{ checkin.RoomName }}</a>
                                        </v-subheader>
                                    </v-flex>
                                </v-layout>
                                <v-layout row>
                                    <v-flex xs3>
                                        <v-subheader>Checkin At</v-subheader>
                                    </v-flex>
                                    <v-flex xs4>
                                        <v-subheader>{{ checkin.CheckinAt }}</v-subheader>
                                    </v-flex>
                                </v-layout>
                                <v-layout row>
                                    <v-flex xs3>
                                        <v-subheader>Departure At</v-subheader>
                                    </v-flex>
                                    <v-flex xs4>
                                        <v-subheader>{{ checkin.DepartureAt }}</v-subheader>
                                    </v-flex>
                                </v-layout>
                                <v-layout row>
                                    <v-flex xs3>
                                        <v-subheader>Note</v-subheader>
                                    </v-flex>
                                    <v-flex xs9>
                                        <v-subheader>{{ checkin.Note }}</v-subheader>
                                    </v-flex>
                                </v-layout>
                                <v-layout row>
                                    <v-flex xs12>
                                        <v-btn dark :disabled="checkin.IsInvoiceClose" color="success" class="mb-4 ml-0" :href="checkin.PayLink">
                                            <span>Pay Invoice</span>
                                            <v-icon right dark>move_to_inbox</v-icon>
                                        </v-btn>
                                        <v-btn dark :disabled="checkin.IsCheckout" color="error" class="mb-4 ml-0" :href="checkin.CheckoutLink">
                                            <span>Checkout</span>
                                            <v-icon right dark>move_to_inbox</v-icon>
                                        </v-btn>
                                        <v-btn :disabled="!checkin.CanChange" dark color="primary" class="mb-4 ml-0" :href="checkin.ChangeLink">
                                            <span>Change Room</span>
                                            <v-icon right dark>refresh</v-icon>
                                        </v-btn>
                                    </v-flex>
                                </v-layout>
                            </v-flex>
                        </v-layout>
                    </v-container>
                </v-card>
            </v-tab-item>
            <v-tab-item>
                <v-card>
                    <v-card-title>
                        <h2 class="card-title mb-0">Invoice</h2>
                    </v-card-title>
                    <v-container fluid grid-list-md>
                        <v-layout row>
                            <v-flex lg12 md12 sm12 xs12>
                                <v-btn dark color="success" class="mb-4 ml-0 float-right no-print" :href="checkin.PayLink">
                                    <span>Pay Or Detail Invoice</span>
                                    <v-icon right dark>move_to_inbox</v-icon>
                                </v-btn>
                                <v-btn dark color="primary" v-show="false" class="mb-4 ml-0 float-right no-print" @click.stop="print">
                                    <span>Print</span>
                                    <v-icon right dark>print</v-icon>
                                </v-btn>
                                <v-data-table v-bind:headers="tableData.headers"
                                              v-bind:items="AllowedItems"
                                              v-bind:total-items="tableData.totalItems"
                                              v-bind:loading="tableData.loading"
                                              hide-actions
                                              class="elevation-1 printarea">
                                    <template slot="items" slot-scope="props">
                                        <tr>
                                            <td>{{ (props.index + 1) }}</td>
                                            <td>{{ props.item.TransactionDate | dateformat }}</td>
                                            <td v-html="props.item.Description"></td>
                                            <td>{{ props.item.AmmountIn | currency }}</td>
                                            <td>{{ props.item.AmmountOut | currency }}</td>
                                        </tr>
                                    </template>
                                    <template slot="footer">
                                        <tr>
                                            <td colspan="4">
                                                <strong>Total Balance</strong>
                                            </td>
                                            <td>{{ TotalBalance | currency }}</td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <strong>Total Pay</strong>
                                            </td>
                                            <td>{{ TotalPay | currency }}</td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <strong>Cashback</strong>
                                            </td>
                                            <td>{{ CashBack | currency }}</td>
                                        </tr>
                                    </template>
                                </v-data-table>
                            </v-flex>
                        </v-layout>
                    </v-container>
                </v-card>
            </v-tab-item>
        </v-tabs-items>
    </div>
</template>
<script>
    import axios from 'axios'
    import { invalid } from 'moment';
    export default {
        data() {
            return {
                deposit: 50000,
                tab: null,
                checkin: {
                    Id: null,
                    Fullname: null,
                    PhotoGuest: null,
                    DetailGuest: null,
                    DetailRoom: null,
                    RoomName: null,
                    CheckinAt: null,
                    DepartureAt: null,
                    InvoiceId: null,
                    InvoiceLink: null,
                    PayLink: null,
                    CheckoutLink: null,
                    ChangeLink: null,
                    IsInvoiceClose: false,
                    IsCheckout: false,
                    CanChange: false,
                    Note: null,
                },
                tableData: {
                    totalItems: 0,
                    loading: false,
                    pagination: {},
                    headers: [
                        { text: '#', sortable: false, align: 'left' },
                        { text: 'Date', sortable: false, align: 'left'  },
                        { text: 'Description', sortable: false, align: 'left' },
                        { text: 'Debit', sortable: false, align: 'left' },
                        { text: 'Kredit', sortable: false, align: 'left' },
                    ],
                    items: []
                }
            }
        },
        filters: {
            currency(val) {
                var Nform = new Intl.NumberFormat('id-ID', { style: 'currency', currency: 'IDR' })

                return Nform.format(val)
            },
            dateformat(val) {
                var momen = moment(val)

                return momen.format('YYYY-MM-DD');
            }
        },
        props: {
            checkid: { type: String, required: true }
        },
        watch: {
            tab: {
                handler() {}
            }
        },
        computed: {
            AllowedItems() {
                var items = this.tableData.items.filter(item => {
                    let kind = item.IdKind

                    return (kind != 98 && kind != 100)
                })

                return items
            },
            TotalBalance() {
                var inVal = 0
                var outVal = 0

                this.tableData.items.forEach((item) => {
                    let kind = item.IdKind

                    if (kind != 98 && kind != 100) {
                        inVal += item.AmmountIn
                        outVal += item.AmmountOut
                    }
                })

                return (inVal - outVal)
            },
            TotalPay() {
                var inVal = 0

                this.tableData.items.forEach((item) => {
                    let kind = item.IdKind

                    if (kind == 100) {
                        inVal += item.AmmountIn
                    }
                })

                return inVal
            },
            CashBack() {
                var inVal = 0
                var cashback = this.tableData.items.filter(item => {
                    return (item.IdKind == 98)
                })

                if (cashback.length == 0) {
                    //calculate cashback
                    inVal = this.TotalBalance - this.TotalPay
                } else {
                    inVal = cashback[0].AmmountOut
                }

                return inVal
            },
            PhotoUrl() {
                if (this.checkin.PhotoGuest)
                    return 'http://localhost.com/Upload/' + this.checkin.PhotoGuest
                else
                    return 'http://localhost.com/images/users.png'
            }
        },
        methods: {
            print() {
                // Print dialog
                window.CS.print().then((e) => { })
            },
            imageError() {
                this.PhotoHash = ''
            },
            getData(response) {
                var data = response.data

                if (data.success) {
                    var check = data.data

                    this.checkin.Id = check.Id
                    this.checkin.CheckinAt = check.CheckinAt
                    this.checkin.DepartureAt = check.DepartureAt
                    this.checkin.Note = check.Note
                    this.checkin.InvoiceId = check.Invoice.Id
                    this.checkin.CanChange = check.CanChange
                    this.checkin.IsCheckout = check.IsCheckout
                    this.checkin.IsInvoiceClose = check.Invoice.IsClosed
                    this.checkin.Fullname = check.Guest.Fullname
                    this.checkin.PhotoGuest = check.Guest.PhotoGuest
                    this.checkin.DetailGuest = check.Guest.DetailLink
                    this.checkin.DetailRoom = check.Room.DetailLink
                    this.checkin.InvoiceLink = check.Invoice.InvoiceLink
                    this.checkin.PayLink = check.Invoice.PayLink
                    this.checkin.CheckoutLink = check.CheckoutLink
                    this.checkin.ChangeLink = check.ChangeLink
                    this.checkin.RoomName = check.Room.RoomName

                    this.getInvoiceApi()
                }
            },
            getDataApi() {
                const id = this.checkid

                axios.post('http://localhost.com/checkin/post/getCheckinDetail', { id })
                    .then(this.getData)
                    .catch(e => { })
            },
            getInvoice(response) {
                var data = response.data

                if (data.success) {
                    var realdata = data.data

                    this.tableData.items = []

                    realdata.Details.forEach(x => this.tableData.items.push(x))
                }
            },
            getInvoiceApi() {
                const id = this.checkin.InvoiceId

                axios.post('http://localhost.com/checkin/post/getInvoiceDetail', { id })
                    .then(this.getInvoice)
                    .catch(e => { })
            },
        },
        mounted() {
            this.getDataApi()
        }
    }
</script>
