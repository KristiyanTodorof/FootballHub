const MA = 'http://localhost:5001/api';
const LA = 'http://localhost:5002/api';
const PA = 'http://localhost:5003/api';

let selLId = null, selLName = '', selLLogo = '', selLSeason = null, pTab = 'scorers';

// Navigation
function show(id, btn) {
    $('.page').removeClass('active');
    $('.nav-btn').removeClass('active');
    $('#' + id).addClass('active');
    $(btn).addClass('active');

    if (id === 'matches') loadMatches();
    if (id === 'standings') loadLeagues('s');
    if (id === 'players') loadLeagues('p');
}

// Matches
function loadMatches() {
    $('#matches-list').html('<div class="loading">Loading matches...</div>');

    $.when(
        $.getJSON(`${MA}/matches/live`),
        $.getJSON(`${MA}/matches/today`)
    ).done(function (liveRes, todayRes) {
        const live = liveRes[0].data || [];
        const today = todayRes[0].data || [];
        const all = [...live, ...today.filter(m => m.status !== 'Live')];

        if (!all.length) {
            $('#matches-list').html('<div class="empty">NO MATCHES TODAY</div>');
            return;
        }

        const liveM = all.filter(m => m.status === 'Live');
        const schedM = all.filter(m => m.status === 'Scheduled');
        const finM = all.filter(m => m.status === 'Finished');

        let html = '';
        if (liveM.length) html += matchGroup('Live Now · ' + liveM.length, liveM);
        if (schedM.length) html += matchGroup('Upcoming', schedM);
        if (finM.length) html += matchGroup('Finished', finM);

        $('#matches-list').html(html);
    }).fail(function () {
        $('#matches-list').html('<div class="empty">FAILED TO LOAD</div>');
    });
}

function matchGroup(title, matches) {
    return `<div class="match-group">
        <div class="match-group-title">${title}</div>
        ${matches.map(renderMatch).join('')}
    </div>`;
}

function renderMatch(m) {
    const isLive = m.status === 'Live';
    const isFin = m.status === 'Finished';
    const goals = (m.events || []).filter(e => e.type === 'Goal');

    const timeTag = isLive
        ? `<span class="time-tag live">● LIVE ${m.minute || ''}'</span>`
        : isFin
            ? `<span class="time-tag fin">FT</span>`
            : `<span class="time-tag sched">${new Date(m.kickOff).toLocaleTimeString('en-GB', { hour: '2-digit', minute: '2-digit' })}</span>`;

    const scoreHtml = (isLive || isFin)
        ? `<div class="score-nums">${m.homeScore} — ${m.awayScore}</div>`
        : `<div class="score-vs">VS</div>`;

    const goalsHtml = goals.length
        ? `<div class="match-goals">${goals.map(g => `<span class="goal-tag">⚽ ${g.playerName} ${g.minute}'</span>`).join('')}</div>`
        : '';

    return `<div class="match-card ${isLive ? 'live' : ''}">
        <div class="match-inner">
            <div class="team-block">
                <span class="team-name">${m.homeTeamName}</span>
            </div>
            <div class="score-center">
                ${timeTag}
                ${scoreHtml}
            </div>
            <div class="team-block away">
                <span class="team-name">${m.awayTeamName}</span>
            </div>
        </div>
        <div class="match-footer">
            <span class="match-league">${m.leagueName.toUpperCase()}</span>
            ${goalsHtml}
        </div>
    </div>`;
}

// Leagues
function loadLeagues(t) {
    $.getJSON(`${LA}/leagues`).done(function (res) {
        const leagues = res.data || [];
        if (!leagues.length) return;

        if (!selLId) {
            selLId = leagues[0].id;
            selLName = leagues[0].name;
            selLLogo = leagues[0].logoUrl;
            selLSeason = leagues[0].season;
        }

        const html = leagues.map(l =>
            `<button class="tab ${l.id === selLId ? 'active' : ''}"
                onclick="selLeague(${l.id}, '${l.name}', '${l.logoUrl}', ${l.season}, '${t}', this)">
                ${l.name}
            </button>`
        ).join('');

        if (t === 's') {
            $('#league-tabs').html(html);
            loadStandings();
        } else {
            $('#pl-league-tabs').html(html);
            loadPlayers();
        }
    });
}

function selLeague(id, name, logo, season, t, btn) {
    selLId = id;
    selLName = name;
    selLLogo = logo;
    selLSeason = season;

    const tabsId = t === 's' ? '#league-tabs' : '#pl-league-tabs';
    $(tabsId + ' .tab').removeClass('active');
    $(btn).addClass('active');

    if (t === 's') loadStandings();
    else loadPlayers();
}

// Standings
function loadStandings() {
    $('#standings-body').html('<div class="loading">Loading...</div>');

    $.getJSON(`${LA}/leagues/${selLId}/standings`).done(function (res) {
        const standings = res.data || [];

        if (!standings.length) {
            $('#standings-body').html('<div class="empty">NO STANDINGS DATA</div>');
            return;
        }

        const rows = standings.map((s, i) => `
            <div class="stand-row ${i < 3 ? 'top3' : ''}">
                <span class="spos">${s.position}</span>
                <span class="sname">${s.teamName}</span>
                <span>${s.played}</span>
                <span>${s.won}</span>
                <span>${s.drawn}</span>
                <span>${s.lost}</span>
                <span>${s.goalDifference > 0 ? '+' + s.goalDifference : s.goalDifference}</span>
                <span class="spts">${s.points}</span>
            </div>
        `).join('');

        $('#standings-body').html(`
            <div class="league-info">
                <img src="${selLLogo}" onerror="this.style.display='none'">
                <span>${selLName}</span>
                <small>Season ${selLSeason}</small>
            </div>
            <div class="stand-wrap">
                <div class="stand-head-row">
                    <span class="hn">#</span>
                    <span class="hn">Club</span>
                    <span>MP</span><span>W</span><span>D</span><span>L</span><span>GD</span><span>Pts</span>
                </div>
                ${rows}
            </div>
        `);
    }).fail(function () {
        $('#standings-body').html('<div class="empty">FAILED TO LOAD</div>');
    });
}

// Players
function switchPTab(t, btn) {
    pTab = t;
    $('#tab-sc, #tab-as').removeClass('active');
    $(btn).addClass('active');
    loadPlayers();
}

function loadPlayers() {
    $('#players-list').html('<div class="loading">Loading...</div>');

    const url = pTab === 'scorers'
        ? `${PA}/players/top-scorers?leagueId=${selLId}&season=${selLSeason}`
        : `${PA}/players/top-assists?leagueId=${selLId}&season=${selLSeason}`;

    $.getJSON(url).done(function (res) {
        const players = res.data || [];

        if (!players.length) {
            $('#players-list').html('<div class="empty">NO DATA</div>');
            return;
        }

        const html = players.map((p, i) => {
            const s = p.statistics && p.statistics[0];
            const statsHtml = s ? `
                <div class="pstats">
                    <div class="pstat">
                        <div class="pstat-val ${pTab === 'scorers' ? 'hi' : ''}">${s.goals}</div>
                        <div class="pstat-lbl">Goals</div>
                    </div>
                    <div class="pstat">
                        <div class="pstat-val ${pTab === 'assists' ? 'hi' : ''}">${s.assists}</div>
                        <div class="pstat-lbl">Assists</div>
                    </div>
                    <div class="pstat">
                        <div class="pstat-val">${s.appearances}</div>
                        <div class="pstat-lbl">Apps</div>
                    </div>
                </div>` : '';

            return `<div class="player-row">
                <div class="prank ${i < 3 ? 'top' : ''}">${i + 1}</div>
                <img class="pphoto" src="${p.photoUrl}"
                    onerror="this.src='https://via.placeholder.com/42/0d0f16/4a4f63?text=?'">
                <div class="pinfo">
                    <div class="pname">${p.fullName}</div>
                    <div class="pmeta">${p.teamName.toUpperCase()} · ${p.position.toUpperCase()} · ${p.age}Y</div>
                </div>
                ${statsHtml}
            </div>`;
        }).join('');

        $('#players-list').html(html);
    }).fail(function () {
        $('#players-list').html('<div class="empty">FAILED TO LOAD</div>');
    });
}

// Auto-refresh live matches every 30 seconds
setInterval(function () {
    if ($('#matches').hasClass('active')) loadMatches();
}, 30000);

// Initial load
loadMatches();