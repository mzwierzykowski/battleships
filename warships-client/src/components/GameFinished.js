import './GameFinished.css'
function GameFinished() {
    return (
        <div className="game-finished-content">
            <div className="title">
                <h3>Victory!</h3>
            </div>
            <div className="text-content">
                <p>
                    Congratulations!<br/>
                    You've won.<br/>
                    Hit 'New Game' button to start again
                </p>
            </div>
            <p></p>
        </div>
    );
}

export default GameFinished;