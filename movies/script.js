var server = "https://api.themoviedb.org/3/";
var apiKey = '93f8a606fa691c5c01dacbfef318a0bb';
var searchEnpoint = "search/movie";
var search = function (value) {
    var xhr = new XMLHttpRequest();
    var url = server + "search/movie?api_key=" + apiKey + "&query=" + value;
    xhr.open('GET', url);
    xhr.onload = function () {
        var data = JSON.parse(this.response);
        if (data.results.length !== 0) {
            var resultMovie_1 = data.results[0];
            console.log(resultMovie_1);
            console.log("Getting data for the movie");
            var movieXhr = new XMLHttpRequest();
            var movieUrl = server + "movie/" + resultMovie_1.id + "?api_key=" + apiKey;
            movieXhr.open('GET', movieUrl);
            movieXhr.onload = function () {
                var movieData = JSON.parse(this.response);
                console.log(movieData);
                console.log("Getting people for the movie");
                var peopleXhr = new XMLHttpRequest();
                var peopleUrl = server + "movie/" + resultMovie_1.id + "/credits?api_key=" + apiKey;
                peopleXhr.open('GET', peopleUrl);
                peopleXhr.onload = function () {
                    var data = JSON.parse(this.response);
                    console.log(data);
                    data.cast.sort(function (f, s) { return f.order - s.order; });
                    var mainActors = data.cast.slice(0, 6);
                    var characters = mainActors.map(function (actor) { return ({
                        name: actor.character,
                        actor: actor.name,
                        image: actor.profile_path
                    }); });
                    var directors = data.crew
                        .filter(function (person) { return person.department === "Directing" && person.job === "Director"; })
                        .map(function (person) { return person.name; });
                    var directedBy = directors.join(" & ");
                    var writers = data.crew
                        .filter(function (person) { return person.department === "Writing" && person.job === "Writer"; })
                        .map(function (person) { return person.name; });
                    var writtenBy = writers.join(" & ");
                    var movie = {
                        id: movieData.id,
                        title: movieData.title,
                        tagline: movieData.tagline,
                        releaseDate: new Date(movieData.release_date),
                        posterUrl: movieData.poster_path,
                        backdropUrl: movieData.backdrop_path,
                        overview: movieData.overview,
                        runtime: movieData.runtime,
                        characters: characters,
                        directedBy: directedBy,
                        writenBy: writtenBy
                    };
                    showResults(movie);
                };
                peopleXhr.send();
            };
            movieXhr.send();
        }
        else {
            console.log("not Found");
        }
    };
    xhr.send();
};
var getCastElements = function (movie) { return movie.characters.map(function (character) {
    var image = character.image
        ? "<img src=\"http://image.tmdb.org/t/p/w185/" + character.image + "\" class=\"image\" />"
        : "<img src=\"https://via.placeholder.com/185x277.png?text=No+Image\" class=\"image\" />";
    return "<div class=\"character\">\n            " + image + "\n            <div class=\"actor\">" + character.actor + " <br /> as " + character.name + "</div>\n        </div>";
}).join(""); };
var getRuntime = function (_a) {
    var runtime = _a.runtime;
    if (!runtime) {
        // do not display the runtime field if we don't know the runtime
        return "";
    }
    if (runtime < 60) {
        return "<div class=\"runtime\"><strong>Run time: </strong>" + runtime + "min";
    }
    var hours = runtime / 60 | 0;
    var minutes = runtime % 60;
    return "<div class=\"runtime\"><strong>Run time: </strong>" + hours + "h " + minutes + "min";
};
var showResults = function (movie) {
    var backdropUrl = "http://image.tmdb.org/t/p/w1280/" + movie.backdropUrl;
    document.body.style.backgroundImage = "url('" + backdropUrl + "')";
    var result = document.getElementById("result");
    result.innerHTML = "\n    <div class=\"main-part\">\n        <div class=\"title\">" + movie.title + " (" + movie.releaseDate.getFullYear() + ")</div>\n        <img src=\"http://image.tmdb.org/t/p/w500/" + movie.posterUrl + "\" class=\"poster\" />\n        <div class=\"tagline\">" + movie.tagline + "</div>\n    </div>\n    <div class=\"details-part\">\n        <div class=\"overview\"><strong>Plot summary: </strong>" + movie.overview + "</div>\n        <div class=\"director\"><strong>Directed By: </strong>" + movie.directedBy + "</div>\n        <div class=\"screenplay\"><strong>Written By: </strong>" + movie.writenBy + "</div>\n        " + getRuntime(movie) + "\n        <div class=\"cast\">\n            <div><strong>Starring:</strong></div>\n            " + getCastElements(movie) + "\n        </div>\n    </div>\n";
    //document.body.style.opacity = "0.2";
    //     const output = document.getElementById("results");
    //     output.innerHTML = "";
    //     for (const result of resArray) {
    //         const element = document.createElement("div");
    //         element.className = "movie";
    //         element.innerHTML = `<span class="index">${result.index}</span>
    // <span class="title">${result.title}</span>
    // <span class="page-id">${result.pageid}</span>`
    //         output.appendChild(element);
    //     }
};
document.addEventListener("DOMContentLoaded", function () {
    document.getElementById("search").addEventListener("click", function () {
        var movieInput = document.getElementById("title");
        var movieTitle = movieInput.value;
        search(movieTitle);
    });
    document.getElementById("title").addEventListener("keyup", function (event) {
        if (event.key !== "Enter") {
            return;
        }
        document.getElementById("search").click();
        event.preventDefault();
    });
});
// ?action=query&
// origin=*&
// format=json&
// generator=search&
// gsrnamespace=0&
// gsrlimit=50&
// gsrsearch=%27New_England_Patriots%27
