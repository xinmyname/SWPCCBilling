# CoffeeScript

App = Ember.Application.create()

App.Fee = Ember.Object.extend({})

App.Fee.reopenClass
    loadAll: ->
        return $.getJSON("fees").then (response) ->
            fees = Em.A()
            response.forEach (child) ->
                fees.pushObject App.Fee.create(child)
            return fees

App.FeesRoute = Ember.Route.extend
    model: ->
        App.Fee.loadAll()

App.Router.map ->
    this.resource('families')
    this.resource('fees')
    this.resource('payments')
    this.resource('discounts')
    this.resource('ledger')
    this.resource('reports')
    this.resource('invoicing')
    this.resource('tools')

