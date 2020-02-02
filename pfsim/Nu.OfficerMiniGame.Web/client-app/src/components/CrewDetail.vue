<template>
    <div class="card">
        <div class="container">
            <div class="form-grid-container">

                <div>
                    <h3>{{crew.name}}</h3>
                </div>
                <div>
                    <button style="align-self:center" class="update-button" v-on:click="saveCrewMember(crew)"> </button>
                    <button style="align-self:center" class="delete-button" v-on:click="deleteCrew()"></button>
                </div>

                <div>
                    <table align="center">
                        <tr>
                            <td style="text-align:right">
                                <label class="stat-label">Craft (Carpentry): </label>
                            </td>
                            <td>
                                <div><input type="number" style="float:left" v-model="crew.skills.craftCarpentry" /></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:right">
                                <div><label class="stat-label">Craft (Cooking): </label></div>
                            </td>
                            <td>
                                <div><input type="number" v-model="crew.skills.craftCooking" /></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:right">
                                <div><label class="stat-label">Craft (Sails): </label></div>
                            </td>
                            <td>
                                <div><input type="number" v-model="crew.skills.craftSails" /></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:right">
                                <div><label class="stat-label">Craft (Ship): </label></div>
                            </td>
                            <td>
                                <div><input type="number" v-model="crew.skills.craftShip" /></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:right">
                                <div><label class="stat-label">Diplomacy: </label></div>
                            </td>
                            <td>
                                <div><input type="number" v-model="crew.skills.diplomacy" /></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:right">
                                <div><label class="stat-label">Heal: </label></div>
                            </td>
                            <td>
                                <div><input type="number" v-model="crew.skills.heal" /></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:right">
                                <div><label class="stat-label">Intimidate: </label></div>
                            </td>
                            <td>
                                <div><input type="number" v-model="crew.skills.intimidate" /></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:right">
                                <div><label class="stat-label">Knowledge (Engineering): </label></div>
                            </td>
                            <td>
                                <div><input type="number" v-model="crew.skills.knowledgeEngineering" /></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:right">
                                <div><label class="stat-label">Perception: </label></div>
                            </td>
                            <td>
                                <div><input type="number" v-model="value.skills.perception" /></div>
                            </td>
                        </tr>

                        <tr>
                            <td style="text-align:right">
                                <div><label class="stat-label">Perform: </label></div>
                            </td>
                            <td>
                                <div><input type="number" v-model="crew.skills.perform" /></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:right">
                                <div><label class="stat-label">Profession (Merchant): </label></div>
                            </td>
                            <td>
                                <div><input type="number" v-model="crew.skills.professionMerchant" /></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:right">
                                <div><label class="stat-label">Profession (Sailor): </label></div>
                            </td>
                            <td>
                                <div><input type="number" v-model="crew.skills.professionSailor" /></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:right">
                                <div><label class="stat-label">Survival: </label></div>
                            </td>
                            <td>
                                <div><input type="number" v-model="crew.skills.survival" /></div>
                            </td>
                        </tr>
                    </table>

                </div>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        name: "CrewDetail",
        props: {
            value: { type: String }
        },
        data: function () {
            return {
                crew: {}
            }
        },
        watch: {
            value: function () {
                this.load(this.value);
            }
        },
        mounted: function () {
            this.load(this.value);
        },
        methods: {
            load(name) {
                var self = this;
                if (this.value.length > 0) {
                    fetch('/CrewMemberStats?name=' + name).then(r => r.json())
                        .then(d => {
                            self.crew = d
                        });
                }

            },
            saveCrewMember(crewMember) {
                fetch('/CrewMemberStats/Update', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(crewMember)
                });
            },
            deleteCrew() {
                if (confirm("Do the thing?")) {
                    this.$emit("delete", this.value.name);
                }
            }
        }
    }
</script>

<style>
</style>