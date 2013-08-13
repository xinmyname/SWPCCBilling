# CoffeeScript

App = Ember.Application.create()

App.Router.map ->
    this.resource('families')
    this.resource('fees')
    this.resource('payments')
    this.resource('discounts')
    this.resource('ledger')
    this.resource('reports')
    this.resource('invoicing')
    this.resource('tools')

