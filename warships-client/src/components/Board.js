import './Board.css'
import Tile from "./Tile";

function Board({points, dimension, handlePointClick}) {
    const renderedTiles =  points.map((point) => {
                return <Tile key={point.id} point={point} handlePointClick={handlePointClick}/>
            });
    const boardStyle = {
        width: 52 * dimension.width,
        height: 52 * dimension.height,
    }

    return (
        <div className="board-area" style={boardStyle}>
            <div className="board">
                {renderedTiles}
            </div>
        </div>
    );
}

export default Board;