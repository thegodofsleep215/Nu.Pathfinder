<template>
    <div>
        <ObjectEditor title="Voyages"
                      url="Voyage"
                      detailComponent="VoyageDetail.vue"></ObjectEditor>
    </div>


</template>

<script>
    import ObjectEditor from './Legos/ObjectEditor.vue'
    export default {
        name: "Voyages",
        components: {
            ObjectEditor
        },
    }
</script>

<style>
</style>