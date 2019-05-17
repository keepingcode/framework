<template lang="pug">
  div(
    xs12
    sm12
    md8
    offset-md2
    v-if="hasForm()"
  )
    h6(class="no-margin") {{ form.title }}

    form(:ref="formName" @submit.prevent="onSubmit")
      div(
        row
        v-for="field in form.fields"
        :key="field.name"
      )
        component(:is="dynamicComponent(field)" :field="field" :ref="field.name")

      div(class="btns q-pa-lg")
        q-btn(
          color="secondary"
          @click="submit"
        ) {{ action.title }}

        q-btn(
          @click="clear()"
        ) Limpar

        q-btn(
          v-if="false"
          @click="goBack()"
          glossy
        ) Cancelar

</template>

<script>
export default {
  data: () => ({
    drawer: true
  }),

  props: ['action'],

  computed: {
    form () {
      var form = this.$paper.form.getFilters()
      return form
    },

    formName () {
      var formName = 'form-' + this.form.name
      return formName
    }
  },

  methods: {
    hasForm () {
      var form = this.$paper.form.hasFilters()
      return form
    },

    dynamicComponent (field) {
      var component = this.$paper.form.dynamicComponent(field, this.action.name)
      return component
    },

    submit () {
      var form = this.$refs[this.formName]
      this.$paper.form.submit(this.action, form)
    },

    clear () {
      this.$children.forEach(children => {
        if (children.clear) {
          children.clear()
        }
      })
      var form = this.$refs[this.formName]
      form.reset()
    },

    goBack () {
      this.$router.go(-1)
    }
  }
}
</script>

<style lang="stylus">
.btns
  .q-btn
    margin 5px
  .btn-fixed-width
    width 200px
.no-margin
  margin-top: 0px;
  margin-bottom: 0px;
</style>
