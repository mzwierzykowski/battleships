import './GameStats.css'

function GameStats({stats}) {
    const shipStats = stats.shipStats.map((shipStat) => {
        return (<tr className="ship-stat" key={shipStat.type}>
            <td>{shipStat.type}</td> 
            <td>{shipStat.totalCount}</td>
            <td>{shipStat.leftInGameCount}</td>
        </tr>);
    });

    let content = <div></div>;
    if(stats.shotsFired > 0) {
        content = (
            <div>
                <div className="ship-stats">
                    <table>
                        <thead>
                            <tr>
                                <th>Ship type</th>
                                <th>Count</th>
                                <th>Left</th>
                            </tr>
                        </thead>
                        <tbody>
                            {shipStats}
                        </tbody>
                    </table>
                </div>
                <div className="game-counters">
                    <table>
                        <thead>
                            <tr>
                                <th>Shoots</th> 
                                <th>Hits</th>
                                <th>Misses</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>{stats.shotsFired}</td>
                                <td>{stats.totalHits}</td>
                                <td>{stats.totalMiss}</td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>

        );
    }
    return (
        <div className="game-stats-container">
            {content}
        </div>
    );
}

export default GameStats