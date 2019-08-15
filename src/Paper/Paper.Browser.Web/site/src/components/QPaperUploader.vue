<template lang="pug">
  div(class="q-pa-md")
    q-uploader(
      :factory="uploadFile"
      ref="uploader"
      auto-upload
      :label="widget.properties.title"
      stack-label="upload image"
    )
</template>

<script>
export default {
  props: ['widget'],

  data () {
    return {
      uploadPercentage: 0
    }
  },

  methods: {
    uploadFile (file) {
      return new Promise((resolve, reject) => {
        var formData = new FormData()
        formData.append('file', file)
        resolve({
          url: 'http://localhost:5000/api/fileuploader/upload',
          method: 'POST',
          body: formData
        })
      })
    }
  }
}
</script>
