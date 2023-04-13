import './Tile.css';

function Tile({point, handlePointClick}) {
    const tileColors = [
        '#f7ffff',
        '#99bbff',
        '#360c0c'
    ]
    const handleClick = () => {
        if(point.state === 0) {
            handlePointClick(point.id);
        }
    }
    return (
        <div className="tile" onClick={handleClick}
            style={{backgroundColor: tileColors[point.state]}}>
        </div>
    );
}

export default Tile;