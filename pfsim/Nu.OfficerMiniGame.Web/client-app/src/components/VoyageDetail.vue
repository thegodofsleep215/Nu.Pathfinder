<template>
    <div class="card">
        <div class="container">
            <div class="form-grid-container">
                <div>
                    <h3 style="display: inline">{{value}}</h3>
                </div>
                <div style="grid-column-start: 1">
                    <button v-on:click="gotoSailing">Go Sailing!</button>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import Sailing from './Sailing.vue'
    export default {
        name: "VoyageDetail",
        props: {
            value: { type: String }
        },
        components: {
            Sailing
        },
        data: function () {
            return {
                voyage: {},
            }
        },
        mounted: function () {
            this.load(this.value);
        },
        watch: {
            value: function () {
                this.load(this.value);
            },
        },
        methods: {
            load(name) {
                var self = this;
                if (this.value.length > 0) {
                    fetch('/Voyage?name=' + name).then(r => r.json())
                        .then(d => {
                            self.voyage = d;
                            self.parseDate();
                        });
                }

            },
            updateVoyage(v) {
                fetch('/Voyage/Update', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(v)
                });
            },
            deleteVoyage() {
                if (confirm("Do the thing?")) {
                    this.$emit("delete-voyage", this.value.name);
                    self.value = undefined;
                }
            },
            gotoSailing() {
                this.$router.push({ path: "Sailing/" + this.voyage.name, params: { id: this.voyage.name } });
            },
            hoistAnchor() {
                if (this.voyage.shipLoadouts === undefined || this.voyage.shipLoadouts.length == 0) {
                    alert("You must select at least one loadout.")
                }
                else if (this.voyage.daysPlanned <= 0) {
                    alert("Days Planned must be greater than 0.")
                }
                else {
                    if (confirm("Hoisting Anchor means the voyage can no logger be edited.")) {
                        this.voyage.underweigh = true;
                        this.updateVoyage(this.voyage);
                        this.gotoSailing();
                    }
                }
            }
        }
    }
</script>