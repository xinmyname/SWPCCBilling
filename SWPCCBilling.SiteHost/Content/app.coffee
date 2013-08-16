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
        controller.set('fee', App.Fee.create())

App.FeesController = Ember.ObjectController.extend
    add : ->
        newFee = App.Fee.save(this.fee)
        this.get('controllers.fees').pushObject(newFee)
        this.transitionToRoute('fees')

App.Router.map ->
    this.resource 'families'
    this.resource 'fees', ->
        this.route 'add'
    this.resource 'payments'
    this.resource 'discounts'
    this.resource 'ledger'
    this.resource 'reports'
    this.resource 'invoicing'
    this.resource 'tools'

