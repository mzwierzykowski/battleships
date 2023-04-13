import './App.css';
import { useState } from "react";
import Board from "./components/Board";
import GameStats from "./components/GameStats";
import axios from "axios";
import GameFinished from './components/GameFinished';



function App() {
    const [boardPoints, setBoardPoints] = useState([]);
    const [boardDimension, setBoardDimension] = useState({width: 0, height: 0});
    const [isFinished, setIsFinished] = useState(false);
    const [stats, setGameStats] = useState({shipStats: [], shotsFired:0, totalHits:0, totalMiss:0});

    const handleClick = async () => {
        var response = await axios.get('http://localhost:5248/game')
        .catch((error) => {
            console.log('Error on API call: ' + error.message);
        });
        if(response) {
            updateGameState(response.data);
        }
    }

    const updateGameState = (gameState) => {
        setIsFinished(gameState.isfinished);
        setBoardPoints(gameState.board.points);
        setBoardDimension(gameState.board.dimension)
        setGameStats(gameState.stats);
    }
    
    const handlePointClick = async (pointId) => {
        if(isFinished === false) {
            var response = await axios.post('http://localhost:5248/game', {
                pointId
            })
            .catch((error) => {
                console.log('Error on API call: ' + error.message);
            });
            if(response) {
                updateGameState(response.data);
            }
        }
    }

    let gameFinishedContent = <div></div>
    if (isFinished) {
        gameFinishedContent = <GameFinished/>
    }

    return (
        <div className="components">
            <div className="board-component">
                <Board dimension={boardDimension} points={boardPoints} handlePointClick={handlePointClick}/>
            </div>
            <div className="panel">
                <div className="game-finished">{gameFinishedContent}</div>
                <button className="new-game-button" onClick={handleClick}>
                    <div className="button-text">New game</div>
                </button>
                <GameStats stats={stats}/>
            </div>

        </div>
    );
}

export default App;