<template>
    <div v-if="game.id">
        <h1>Guess the Genre</h1>
        <image>
            <img v-bind:src="game.image.original" alt="GameSpot.com" @load="existsImage=true"/>
        </image>
        <!--<h3>{{game.genres}}</h3>
        <h3>{{game.id}}</h3>-->
        <h4>
            <button v-on:click="checkAnswer(buttons[0], 0)" :disabled='isDisabled[0]'>{{buttons[0]}}</button>
            <button v-on:click="checkAnswer(buttons[1], 1)" :disabled='isDisabled[1]'>{{buttons[1]}}</button>
            <button v-on:click="checkAnswer(buttons[2], 2)" :disabled='isDisabled[2]'>{{buttons[2]}}</button>
            <button v-on:click="checkAnswer(buttons[3], 3)" :disabled='isDisabled[3]'>{{buttons[3]}}</button>
        </h4>

        <h1>Score: {{scoreCount}} Combo: {{comboCount}}</h1>
        <h1 v-if="answerCorrect && !isHidden">You guessed correctly 😊</h1>
        <h1 v-if="!answerCorrect && !isHidden">You choose the wrong genre 😢</h1>

        <ul v-if="errors && errors.length">
            <li v-for="error of errors">
                {{error.message}}
            </li>
        </ul>

    </div>
</template>

<style>
    img {
        padding: 15px;
        min-width: 256px;
        min-height: 256px;
        max-width: 512px;
        max-height: 512px;
        align-content: center;
    }
</style>

<script>
    import axios from 'axios';

    const config = {
        headers: {
            Authorization: 'Bearer ' + localStorage.getItem('token'),
        },
    };

    export default {
        data() {
            return {
                game: [],
                errors: [],
                genresFullList: [],
                buttons: [],
                answerCorrect: [],
                scoreCount: 0,
                points: 20,
                comboCount: 0,
                isHidden: true,
                existsGenre: false,
                currentLevel: [],
                isDisabled: [],
                existsImage: false,
            }
        },

        async mounted() {
            this.init();
            this.created();
            this.checkAnswer();
        },

        methods: {
            init() {
                this.genresFullList = ["Real-Time", "Strategy", "Compilation", "Adventure", "Golf", "Simulation",
                    "Sports", "Action", "On-Rails", "Shooter", "Arcade", "Driving/Racing", "Beat-'Em-Up", "First-Person",
                    "Shooter", "Flight", "Platformer", "Role-Playing", "Trivia/Board Game", "Turn-Based", "Puzzle", "3D", "2D"];
                this.buttons = ["", "", "", ""];
                this.isDisabled = [false, false, false, false];
            },

            reinitialization() {
                this.genresFullList = ["Real-Time", "Strategy", "Compilation", "Adventure", "Golf", "Simulation",
                    "Sports", "Action", "On-Rails", "Shooter", "Arcade", "Driving/Racing", "Beat-'Em-Up", "First-Person",
                    "Shooter", "Flight", "Platformer", "Role-Playing", "Trivia/Board Game", "Turn-Based", "Puzzle", "3D", "2D"];
                this.isDisabled = [false, false, false, false];
                this.existsImage = true;
            },

            prepareAnswers() {

                let randomButton = Math.floor(Math.random() * this.buttons.length);
                let randomGenre = Math.floor(Math.random() * this.game.genres.length);
                this.buttons[randomButton] = this.game.genres[randomGenre].name;

                for (let x = 0; x < this.genresFullList.length; x++) {
                    for (let y = 0; y < this.game.genres.length; y++) {
                        if (this.genresFullList[x] == this.game.genres[y].name) {
                            delete this.genresFullList[x];
                            break;
                        }
                    }
                }

                for (let index = 0; index < this.buttons.length; index++) {
                    let randomNumber = Math.floor(Math.random() * this.genresFullList.length);
                    while (this.genresFullList[randomNumber] == null && index != randomButton && this.genresFullList[randomNumber] != this.buttons[randomButton]) {
                        randomNumber = Math.floor(Math.random() * this.genresFullList.length);
                        this.buttons[index] = this.genresFullList[randomNumber];
                    }
                    if (this.genresFullList[randomNumber] != "" && index != randomButton && this.genresFullList[randomNumber] != this.buttons[randomButton]) {
                        this.buttons[index] = this.genresFullList[randomNumber];
                        delete this.genresFullList[randomNumber];
                    }
                }
            },

            checkAnswer: function (genre, id) {
                if (!this.existsGenre && this.game && this.game.genres) {
                    for (let index = 0; index < this.game.genres.length; index++) {
                        if (this.game.genres[index].name == genre) {
                            this.isDisabled[id] = true;
                            this.answerCorrect = true;
                            this.isHidden = false;
                            this.scoreCount = this.scoreCount + this.points + (this.comboCount * 5);
                            this.comboCount += 1;
                            this.existsGenre = true;
                            this.updateGameUrl();
                            this.updateCurrentLevel();
                            this.addGame();
                            break;
                        }
                    }
                    if (!this.existsGenre) {
                        this.answerCorrect = false;
                        this.isHidden = false;
                        this.scoreCount -= 5;
                        this.comboCount = 0;
                        this.isDisabled[id] = true;
                        this.updateCurrentLevel();
                    }
                }
            },

            async created() {
                await axios.get('http://localhost:7279/api/GuessTheGame', config)
                    .then(response => {
                        this.game = response.data
                        this.prepareAnswers()
                        this.existsGenre = false;
                    })
                    .catch(e => {
                        this.errors.push(e)
                    }),

                    await axios.get('http://localhost:7279/api/GuessTheGame/currentlevel', config)
                        .then(reponse => {
                            this.currentLevel = reponse.data.forEach(item => {
                                if (localStorage.getItem('idUser') == item.id) {
                                    this.scoreCount = item.score
                                    this.comboCount = item.combo;
                                }
                            });
                        })
                        .catch(e => {
                            this.errors.push(e)
                        })
            },

            async updateCurrentLevel() {
                await axios.put('http://localhost:7279/api/GuessTheGame?score=' + this.scoreCount + '&combo=' + this.comboCount + '&idUser=' + localStorage.getItem('idUser'), null,
                    config)
                    .catch(e => {
                        this.errors.push(e)
                    })
            },

            async updateGameUrl() {
                await axios.get('http://localhost:7279/api/GuessTheGame', config)
                    .then(response => {
                        this.game = response.data,
                        this.existsGenre = false;
                        this.isHidden = true;
                        this.prepareAnswers();
                        this.reinitialization();
                    })
                    .catch(e => {
                        this.errors.push(e);
                    });
            },

            async addGame() {
                await axios.post('http://localhost:7279/api/GuessTheGame', 
                    {
                    "name": this.game.name,
                    "apiId": this.game.id,
                    "idUser": localStorage.getItem('idUser'),
                    },
                    config,
                );
            },
        },
    }

</script>