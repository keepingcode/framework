<template lang="pug">
  div(
    xs12
    sm12
    md8
    offset-md2
  )
    h6(class="no-margin") {{ title }}

    form(:ref="title")
      div(
        row
        v-for="widget in widgets"
        :key="widget.properties.name"
      )
        component(
          :is="$paper.filter.dynamicComponent(widget)"
          :widget="widget"
          :ref="widget.properties.name"
        )

      div(class="btns q-pa-lg")
        q-btn(
          color="secondary"
          v-for="link in links"
          @click="submit(link)"
          :key="link.href"
        ) {{ link.title }}

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

  props: ['widgets', 'title', 'links'],

  methods: {
    async submit (link) {
      var form = this.$refs[this.title]
      this.$paper.form.submit(form, link).then(response => {
        if (!response.ok) {
          this.$q.notify('Erro ao enviar o formulário.')
        }
      })
    },

    clear () {
      this.$children.forEach(children => {
        if (children.clear) {
          children.clear()
        }
      })
      var form = this.$refs[this.title]
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
