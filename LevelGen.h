#ifndef _SIMBASE_H_
#include "sim/simBase.h"
#endif

#include <vector>

class LevelGen : public SimObject {
private:
    typedef SimObject Parent;
    std::vector< std::vector<int> > map;
    int sizeX, sizeY;
    
public:
    LevelGen() {}
    virtual ~LevelGen() {}
    
    int getVal( int, int );
    void initLevel( int, int );
    int checkAlive( int, int );
    void automata(int, int, int);
    void clean();
    
    void placeStuff();
    
    DECLARE_CONOBJECT( LevelGen );
};

ConsoleMethod( LevelGen, getVal, F32, 4, 4, "" ) {
    return object->getVal( dAtoi( argv[2] ), dAtoi( argv[3] ) );
}

ConsoleMethod( LevelGen, initLevel, void, 0, 4, "" ) {
    object->initLevel( dAtoi( argv[2] ), dAtoi( argv[3] ) );
}

ConsoleMethod( LevelGen, automata, void, 5, 5, "" ) {
    object->automata( dAtoi( argv[2] ), dAtoi( argv[3] ), dAtoi( argv[4] ) );
}

ConsoleMethod( LevelGen, placeStuff, void, 0, 0, "" ) {
    object->placeStuff();
}