<template lang="pug">
  q-page(
    padding
    class="justify-center"
  )
    q-card(class="my-card")
      div(class="q-pa-md")
        form(ref="formulario")
          div(
            row
            v-for="widget in widgets"
            :key="widget.properties.name"
          )
            component(
              :is="$paper.form.dynamicComponent(widget)"
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

            q-circular-progress(
              indeterminate
              size="20px"
              color="secondary"
              class="q-ma-md"
              v-if="showProgress"
            )

</template>

<script>
export default {
  data () {
    return {
      showProgress: false
    }
  },

  computed: {
    widgets () {
      var widgets = this.$paper.form.widgets
      return widgets
    },

    links () {
      var links = this.$paper.form.links
      return links
    }
  },

  methods: {
    submit (link) {
      this.showProgress = true
      var form = this.$refs['formulario']
      var formData = new FormData()
      if (form && form.length > 0) {
        for (var i = 0; i < form.length; i++) {
          var field = form[i]
          if (field && field.name && field.value) {
            formData.append(field.name, field.value)
          }
        }
      }
      formData.append('tribunal', 'TRT')
      this.$axios.request({
        url: link.href,
        method: 'POST',
        data: formData,
        config: {
          headers: {
            'Content-Type': 'multipart/form-data',
            'Accept': 'application/json;application/vnd.siren+json;charset=UTF-8;',
            'Access-Control-Expose-Headers': 'Access-Control-*',
            'Access-Control-Allow-Headers': 'Access-Control-*, Origin, X-Requested-With, Content-Type, Accept',
            'Access-Control-Allow-Methods': 'GET, POST, PUT, DELETE, OPTIONS, HEAD',
            'Access-Control-Allow-Origin': '*',
            'Allow': 'GET, POST, PUT, DELETE, OPTIONS, HEAD'
          }
        }
      }).then(response => {
        this.showProgress = false
        this.$q.notify('Arquivo processado com sucesso.')
        return {
          ok: true,
          data: response
        }
      }).catch(error => {
        if (!error.response) {
          return {
            ok: false,
            data: error.response
          }
        }
        console.log('Erro: ', error.response)
        this.showProgress = false
        this.$q.notify(`Erro ao processar o arquivo: ${error.response} `)
        return {
          ok: false,
          data: error.response
        }
      })
    }
  }
}
</script>

<style lang="stylus" scoped>
.my-card
  width 100%
  max-width 500px
  justify-content center
</style>
