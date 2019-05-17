<template lang="pug">
  div(class="q-pa-lg")
    div(
      xs12
      sm12
      md8
      offset-md2
    )
      q-page(class="self-center")

        h6(class="margin-bottom") {{ title }}

        q-table(
          row-key="id"
          :data="items"
          :columns="columns"
          :pagination.sync="pagination"
          :selected.sync="selected"
          selection="multiple"
          table-style="overflow-y:hidden"
          no-data-label="Não existem dados para exibir."
          no-results-label="Nenhum registro foi encontrado."
          loading-label="Carregando..."
          hide-bottom
          dense
        )

          // PAGINAÇÂO
          template(
            slot="top-right"
            slot-scope="props"
          )
            q-btn(
              color="secondary"
              flat
              :disabled="enableFirstPage"
              @click.stop="goToFirstPage()"
            )
              q-icon(name="first_page")
              span(class="gt-sm") Primeira Página

            q-btn(
              color="secondary"
              flat
              :disable="enablePreviousPage"
              @click.stop="goToPreviousPage()"
            )
              q-icon(name="navigate_before")
              span(class="gt-sm") Anterior

            q-btn(
              color="secondary"
              flat
              :disable="enableNextPage"
              @click.stop="goToNextPage()"
            )
              q-icon(name="navigate_next")
              span(class="gt-sm") Próxima

          // SELEÇÃO
          q-tr(
            slot="header"
            slot-scope="props"
          )
            q-th(auto-width)
              q-checkbox(
                v-if="props.multipleSelect"
                v-model="props.selected"
                indeterminate-value="some"
              )

            q-th(
              v-for="col in props.cols"
              :key="col.name"
              :props="props"
            )
              | {{ col.label }}

          // LINHAS CUSTOMIZADAS
          q-tr(
            slot="body"
            slot-scope="rows"
            :props="rows"
          )
            q-td(auto-width)
              q-checkbox(
                color="primary"
                v-model="rows.selected"
              )

            q-td(
              v-for="col in rows.cols"
              :key="col.name"
              :props="rows"
            )
              q-paper-label(
                :name="col.name"
                :value="rows.row[col.name]"
                truncate
              )

            q-td(
              key="actions"
              align="center"
              auto-width
              v-if="showPopover"
            )
              q-btn(
                size="sm"
                flat
                dense
                color="secondary"
                icon="more_vert"
                class="q-mr-xs"
                @click="toggleActionsPopover(rows)"
              )
                q-popover(
                  v-model="displayActionsMenu[rows.key]"
                )
                  q-list(
                    dense
                    link
                  )
                    q-item(
                      v-for="action in actions"
                      :key="action.name"
                      v-close-overlay
                      @click.native="openAction(action, rows.row)"
                    )
                      q-item-main
                        q-item-tile {{ action.title }}

</template>

<style>
</style>

<script>
import QPaperLabel from '../components/PaperLabel.vue'
export default {
  data: () => ({
    pagination: {
      rowsPerPage: 0
    },
    selected: [],
    displayActionsMenu: false
  }),

  computed: {
    items () {
      var items = this.$paper.record.records.properties
      return items
    },

    title () {
      var title = this.$paper.browser.title
      return title
    },

    columns () {
      var columns = []
      var headers = this.$paper.record.records.headers
      if (headers) {
        headers.forEach(header => {
          columns.push({
            name: header.properties.name,
            type: header.properties.dataType,
            label: header.properties.title,
            align: this.$paper.utils.isString(header) ? 'left' : 'center'
          })
        })
      }
      return columns
    },

    firstPage () {
      var firstPage = this.$paper.browser.pagination.firstPage
      return firstPage
    },

    nextPage () {
      var nextPage = this.$paper.browser.pagination.nextPage
      return nextPage
    },

    previousPage () {
      var previousPage = this.$paper.browser.pagination.previousPage
      return previousPage
    },

    enableFirstPage () {
      var hasFirstPage = this.$paper.browser.pagination.hasFirstPage()
      return !hasFirstPage
    },

    enableNextPage () {
      var hasNextPage = this.$paper.browser.pagination.hasNextPage()
      return !hasNextPage
    },

    enablePreviousPage () {
      var hasPreviousPage = this.$paper.browser.pagination.hasPreviousPage()
      return !hasPreviousPage
    },

    showPopover () {
      return this.$paper.browser.hasActions()
    },

    actions () {
      var actions = this.$paper.browser.actions
      return actions
    }
  },

  components: {
    QPaperLabel
  },

  methods: {
    goToFirstPage () {
      this.$paper.browser.pagination.goToFirstPage()
    },

    goToNextPage () {
      this.$paper.browser.pagination.goToNextPage()
    },

    goToPreviousPage () {
      this.$paper.browser.pagination.goToPreviousPage()
    },

    toggleActionsPopover (popoverid) {
      this.displayActionsMenu = !this.displayActionsMenu
    },

    openAction (action, item) {
      this.$paper.browser.setSelected(item)
      this.$paper.browser.openUrl(action.href)
    }
  },

  watch: {
    selected (val) {
      this.$paper.browser.setSelected(val)
    }
  }
}
</script>

<style lang="stylus">
.margin-bottom
  margin-top: 0px;
  margin-bottom: 30px;
</style>
