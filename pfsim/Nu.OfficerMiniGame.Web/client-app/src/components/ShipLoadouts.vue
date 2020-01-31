<template>
    <div>
        <ObjectEditor title="Ship Loadouts"
                      url="ShipLoadout"
                      detailComponent="ShipLoadoutDetails.vue"></ObjectEditor>
    </div>

</template>

<script>
    import ObjectEditor from './Legos/ObjectEditor.vue'
    export default {
        name: 'ShipLoadouts',
        components: {
            ObjectEditor
        }
    }
</script>
