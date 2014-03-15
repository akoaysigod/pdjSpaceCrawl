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
    void circlePack();
    bool pickCircle( int, int );
    
    void placeStuff();
    
    DECLARE_CONOBJECT( LevelGen );
};

//I know I shouldn't do this but I'm doing it anyway
class CircleGen : public SimObject {
private:
    typedef SimObject Parent;
    std::vector< std::vector<int> > circles;

public:
    CircleGen() {
        circles.resize( 15, std::vector<int>( 15, 0 ) ); 
    }
    virtual ~CircleGen() {}

    bool pickCircle( int x, int y ) {
        if ( circles[y][x] == 0 ) {
            circles[y][x] = 1;
            circles[y + 1][x] = 1;
            circles[y - 1][x] = 1;
            circles[y][x + 1] = 1;
            circles[y][x - 1] = 1;
            return true;
        } else {
            return false;
        }
    }

    DECLARE_CONOBJECT( CircleGen );
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

ConsoleMethod( CircleGen, pickCircle, bool, 4, 4, "" ) {
    return object->pickCircle( dAtoi( argv[2] ), dAtoi( argv[3] ) );
}