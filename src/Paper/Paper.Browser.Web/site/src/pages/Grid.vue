<template lang="pug">
  div(class="q-pa-lg")
    div(
      xs12
      sm12
      md8
      offset-md2
    )
      q-page(class="self-center")

        h6(class="margin-bottom") {{ $paper.browser.title }}

        q-table(
          dense
          row-key="id"
          :data="items"
          :columns="columns"
          :pagination.sync="pagination"
          :loading="loading"
          no-data-label="Não existem dados para exibir."
          no-results-label="Nenhum registro foi encontrado."
          loading-label="Carregando..."
          hide-bottom
          :class="menuFixedColumnClass"
          :sort-method="sort"
          binary-state-sort
        )
          // PAGINAÇÃO
          template(
            v-slot:top
          )
            div
            q-space
            q-btn(
              flat
              dense
              color="primary"
              :disable="!$paper.browser.pagination.hasFirstPage()"
              @click="goToPage('first')"
            )
              q-icon(name="first_page")
              span(class="gt-sm") Primeira Página

            q-btn(
              class="on-right"
              flat
              dense
              color="primary"
              :disable="!$paper.browser.pagination.hasPreviousPage()"
              @click="goToPage('prev')"
            )
              q-icon(name="navigate_before")
              span(class="gt-sm") Anterior

            q-btn(
              class="on-right"
              flat
              dense
              color="primary"
              :disable="!$paper.browser.pagination.hasNextPage()"
              @click="goToPage('next')"
            )
              q-icon(name="navigate_next")
              span(class="gt-sm") Próxima

          // LINHAS CUSTOMIZADAS
          template(
            v-slot:body-cell="props"
          )
            q-td(
              v-if="!isMenuColumn(props.col)"
              :align="props.col.align_data"
              :style="hasCelLink(props.row, props.col) ? 'cursor: pointer;' : ''"
            )
              a(
                href="#"
                @click="openCelView(props.row, props.col)"
                v-if="hasCelLink(props.row, props.col)"
              )
                q-paper-label(
                  :name="props.col.name"
                  :value="props.row.properties[props.col.name]"
                  truncate
                  link
                )

                q-tooltip {{ getCelTootip(props.row, props.col) }}

                q-paper-visibility-btn(
                  :text="String(props.row.properties[props.col.name])"
                  :title="props.col.label"
                )

              q-paper-label(
                :name="props.col.name"
                :value="props.row.properties[props.col.name]"
                truncate
                v-else
              )

              q-paper-visibility-btn(
                :text="String(props.row.properties[props.col.name])"
                :title="props.col.label"
              )

            // Coluna do Menu
            q-td(
              v-else
              auto-width
            )
              q-btn(
                size="sm"
                flat
                dense
                icon="more_vert"
                class="q-mr-xs"
              )
                q-menu
                  q-list(dense)
                    q-item(
                      v-for="action in $paper.browser.actions"
                      :key="action.name"
                      v-close-popup
                      clickable
                      @click.native="openAction(action, props.row.properties)"
                    )
                      q-item-section
                        q-item-label {{ action.title }}

</template>

<script>
import QPaperLabel from '../components/QPaperLabel.vue'
import QPaperVisibilityBtn from '../components/QPaperVisibilityButton.vue'
export default {
  data: () => ({
    pagination: {
      rowsPerPage: 15
    },
    selected: [],
    displayActionsMenu: false,
    menuColumnName: '__menu',
    loading: false
  }),

  computed: {
    items () {
      var items = []
      var properties = this.$paper.data.records.properties
      properties.forEach((property, index) => {
        var record = this.$paper.data.records.getRecord(index)
        items.push({
          properties: property,
          links: record.links
        })
      })
      return items
    },

    columns () {
      var columns = []
      var headers = this.$paper.data.records.headers
      if (headers) {
        headers.forEach(header => {
          var isTextDataType = (header.properties.dataType === 'text') ||
            (header.properties.dataType === 'string')
          columns.push({
            name: header.properties.name,
            type: header.properties.dataType,
            label: header.properties.title,
            align: 'left',
            align_data: isTextDataType ? 'left' : 'center',
            sortable: header.hasLinkByRel('sort')
          })
        })
        var hasActions = this.$paper.browser.hasActions()
        if (hasActions) {
          columns.push({
            name: this.menuColumnName,
            align: 'center'
          })
        }
      }
      return columns
    },

    menuFixedColumnClass () {
      var hasActions = this.$paper.browser.hasActions()
      return hasActions ? 'last-sticky-column-table' : ''
    }
  },

  components: {
    QPaperLabel,
    QPaperVisibilityBtn
  },

  methods: {
    toggleActionsPopover (popoverid) {
      this.displayActionsMenu = !this.displayActionsMenu
    },

    openAction (action, item) {
      this.$paper.browser.setSelected(item)
      this.$paper.browser.openUrl(action.href)
    },

    isMenuColumn (column) {
      var isMenuColumn = column.name === this.menuColumnName
      return isMenuColumn
    },

    hasCelLink (row, col) {
      var hasCelLink = row.links && row.links.some(link => link.rel.includes(col.name))
      return hasCelLink
    },

    getRow (row) {
      var record = this.$paper.data.records.getRecord(row.__index)
      return record
    },

    getCelTootip (row, col) {
      var celLink = row.links.find(link => link.rel.includes(col.name))
      return celLink ? celLink.title : ''
    },

    openCelView (row, col) {
      var celLink = row.links.find(link => link.rel.includes(col.name))
      this.$paper.browser.openUrl(celLink.href)
    },

    goToPage (page) {
      this.loading = true
      this.$paper.browser.pagination.loadPage(page)
        .then(response => {
          if (!response.ok) {
            this.$q.notify('Erro ao executar a paginação.')
          }
          this.loading = false
        })
    },

    async sort (rows, sortBy, descending) {
      console.log('pressed sort', sortBy)
      this.loading = true
      return this.$paper.browser.sort(sortBy)
        .then(response => {
          console.log('response sort', response)
          if (!response.ok) {
            this.$q.notify('Erro ao executar a ordenação.')
          }
          this.loading = false
          return this.items
        })
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
a
  text-decoration: none;

.margin-bottom
  margin-top: 0px;
  margin-bottom: 30px;

.last-sticky-column-table
  thead tr:last-child th:last-child
    background-color #fff
    opacity 1

  td:last-child
    background-color #fff

  thead tr:last-child th:last-child,
  td:last-child
    position sticky
    right 0
    z-index 1
</style>
