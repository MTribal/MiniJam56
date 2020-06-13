namespace My_Utils.PathFinding
{
    /// <summary>
    /// The type of algorithm to find a path
    /// Normal - Default A*
    /// NonDiagonal - Not allow diagonals
    /// </summary>
    public enum FindPathType { Normal, NonDiagonal };


    /// <summary>
    /// The function to apply to platform node type position when shortest path is required.
    /// RoundDown -> The nearest walkable node in the down direction.
    /// </summary>
    public enum RoundNodeType { None, SearchDown, SearchInAllDirections };


    /// <summary>
    /// When the path will be atualized in path finding scripts
    /// </summary>
    public enum AtualizePathMode { Always, WhenTargetMoves };


    /// <summary>
    /// How the target will be set.
    /// </summary>
    public enum SetTargetMode { Inspector, Code };


    /// <summary>
    /// How the target will be defined.
    /// First -> The first that appears.
    /// Nearest -> The nearest of all.
    /// </summary>
    public enum DefineTargetType { First, Nearest };


    /// <summary>
    /// Different ways to recognize a target.
    /// Tag -> Search for a object in the scene with that tag; Object -> A specific object that need to be set manually.
    /// </summary>
    public enum TargetType { Tag, Object };


    /// <summary>
    /// The behaviours of the AI.
    /// Follow - Follow a path to a target.
    /// Record - Record the inputs and save it.
    /// </summary>
    public enum AIMode { Follow, Record };

    
    /// <summary>
    /// Represents the types of recordings.
    /// IfNotExist - Record if an edge not exist yet.
    /// Override - Override an edge if exist, else create new.
    /// Better - Always remains with the better edge. (Less weight)
    /// </summary>
    public enum RecordType { IfNotExist, Override, Better };


    /// <summary>
    /// Describes how a graph will be allocated.
    /// Four - Use four and then deallocate.
    /// Persistent - Don't deallocate until scene changes.
    /// Allocate take a big time, so don't do this often.
    /// </summary>
    public enum SaveKeyType { Once, Persistent };


    /// <summary>
    /// Represents the type of path finding in a PathFindingManager script.
    /// </summary>
    public enum PathFindingType { Topdown, Platformer, Both };


    /// <summary>
    /// Represents the type of a key. Platformer or Topdown.
    /// </summary>
    public enum DataKeyType { Platformer, Topdown }; 


    /// <summary>
    /// Types of vector path used in path finding result.
    /// FullPath -> All points of the path are returned.
    /// OptimizeSpace -> Remove redundent points to optimize space. Ex: [(0, 1), (1, 2), (2, 3)] => [(0, 3)]
    /// </summary>
    public enum VectorPathType { FullPath, OptimizeSpace};


    /// <summary>
    /// Types of TopdownGrid. 
    /// Dynamic -> The grid is created in the Start() method and its not saved anywhere. 
    /// Managed -> The grid is loaded with a topdownGridKey and saved in a topdownGridKey.
    /// </summary>
    public enum TopdownGridType 
    { 
        Dynamic,
        Managed
    };
}
