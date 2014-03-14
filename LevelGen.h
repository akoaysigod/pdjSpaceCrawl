#ifndef _SIMBASE_H_
#include "sim/simBase.h"
#endif

#include <vector>

class LevelGen : public SimObject {
private:
    typedef SimObject Parent;
    std::vector< std::vector<int> > map;
    int sizeX, sizeY;
    int fillLevel;
    
public:
    LevelGen() {}
    virtual ~LevelGen() {}
    
    int getVal( int, int );
    void finish();
    void initLevel( int, int, int );
    int checkAlive( int, int );
    void automata(int, int, int);
    void clean();
    
    void placeStuff();
    
    DECLARE_CONOBJECT( LevelGen );
};

ConsoleMethod( LevelGen, getVal, F32, 4, 4, "" ) {
    return object->getVal( dAtoi( argv[2] ), dAtoi( argv[3] ) );
}

ConsoleMethod( LevelGen, finish, void, 0, 0, "" ) {
    object->finish();
}

ConsoleMethod( LevelGen, initLevel, void, 5, 5, "" ) {
    object->initLevel( dAtoi( argv[2] ), dAtoi( argv[3] ), dAtoi( argv[4] ) );
}

ConsoleMethod( LevelGen, automata, void, 5, 5, "" ) {
    object->automata( dAtoi( argv[2] ), dAtoi( argv[3] ), dAtoi( argv[4] ) );
}
