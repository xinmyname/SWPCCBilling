# CoffeeScript

App = Ember.Application.create()

App.Fee = Ember.Object.extend
    Id: null,
    Name: null,
    Type: null,
    Amount: null

App.Fee.reopenClass
    loadAll: ->
        return $.getJSON("fees").then (response) ->
            fees = Em.A()
            response.forEach (child) ->
                fees.pushObject App.Fee.create(child)
            return fees
    save: (fee) ->
        $.ajax
            type: 'post',
            url: 'fees/add',
            data:
                Id: fee.Id,
                Name : fee.Name,
                Type: fee.Type,
                Amount: fee.Amount
            async : false

App.FeesRoute = Ember.Route.extend
    model: ->
        App.Fee.loadAll()

App.FeesAddRoute = Ember.Route.extend
    setupController: (controller) ->

App.FeesEditRoute = Ember.Route.extend
    setupController: (controller) ->

App.FeesController = Ember.ObjectController.extend
    add : ->
        $.getJSON("fees/add").then (newFee) =>
            this.get('controllers.fees').pushObject(newFee)
            this.transitionToRoute('fees')
    edit : ->

App.Router.map ->
    this.resource 'families'
    this.resource 'fees', ->
        this.route 'add',
        this.route 'edit', { path:":Id" }
    this.resource 'payments'
    this.resource 'discounts'
    this.resource 'ledger'
    this.resource 'reports'
    this.resource 'invoicing'
    this.resource 'tools'

