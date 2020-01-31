<template>
    <div>
        <ObjectEditor title="Ships"
                      url="ShipStats"
                      detailComponent="ShipStatDetails.vue"></ObjectEditor>
    </div>

</template>

<script>
    import ObjectEditor from './Legos/ObjectEditor.vue'
    export default {
        name: "ShipStatList",
        components: {
            ObjectEditor
        },
    }
</script>