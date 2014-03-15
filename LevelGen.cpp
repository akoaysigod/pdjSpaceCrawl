//why you got to copyright my code michael perry =( 


#include "LevelGen.h"
IMPLEMENT_CONOBJECT( LevelGen );
IMPLEMENT_CONOBJECT( CircleGen );

int LevelGen::getVal( int x, int y ) {
    return map[y][x];
}

void LevelGen::finish() {
    clean();
    //fill();
    placeStuff();
}

void LevelGen::initLevel( int x, int y, int rate ) {
    sizeX = x;
    sizeY = y;
    
    map.resize( sizeY, std::vector<int>( sizeX, 0 ) );

    int fillLevel = rand() % 3 + 3;
    for ( int y = 0; y != sizeY; y++ ) {
        for (int x = 0; x != sizeX; x++ ) {
            if ( x <= fillLevel || y <= fillLevel || x >= sizeX - fillLevel ) {
                map[y][x] = 1;
                continue;
            }
            if ( rand() % 100 < rate ) {
                map[y][x] = 1;
            } else {
                map[y][x] = 0;
            }
        }
    }
}

int LevelGen::checkAlive( int x, int y ) {
    int ret = 0;
    ret += map[y + 1][x];
    ret += map[y - 1][x];
    ret += map[y][x + 1];
    ret += map[y + 1][x + 1];
    ret += map[y - 1][x + 1];
    ret += map[y][x - 1];
    ret += map[y + 1][x - 1];
    ret += map[y - 1][x - 1];
    return ret;
}

void LevelGen::automata( int bRate, int sRate, int it ) {
    std::vector< std::vector<int> > mapCopy = map;
    //mapCopy = map;
    
    for ( int i = 0; i != 1; i++ ) {
        for ( int y = 1; y != sizeY - 1; y++ ) {
            for (int x = 1; x != sizeX - 1; x++ ) {
                int living = checkAlive( x, y );
                if ( map[y][x] == 1 ) {
                    if ( living < sRate ) {
                        mapCopy[y][x] = 0;
                    } else {
                        mapCopy[y][x] = 1;
                    }
                } else {
                    if ( living > bRate ) {
                        mapCopy[y][x] = 1;
                    } else {
                        mapCopy[y][x] = 0;
                    }
                }
            }
        }
        map = mapCopy;
    }
}

void LevelGen::clean() {
    for ( int y = 1; y != sizeY - 1; y++ ) {
        for ( int x = 1; x != sizeX - 1; x++ ) {
            int living = checkAlive( x, y );
            if ( map[y][x] == 1 && living <= 2 ) {
                map[y][x] = 0;
            }
            
            if ( map[y][x] == 0 && living > 6 ) {
                map[y][x] = 1;
            }
        }
    }
}

/*
 3 = item location;
 5 = station down
 6 = station up
 
 30 = spawn point
 31 = fill ^
*/
void LevelGen::placeStuff() {
    for ( int y = 1; y != sizeY - 1; y++ ) {
        int counter = 0;
        for ( int x = 1; x != sizeX - 1; x++ ) {
            if ( map[y - 1][x] == 1 && map[y + 1][x] == 1 ) {
                counter = 0;
                continue;
            }
            
            if ( map[y][x] == 1 ) {
                counter++;
            } else {
                counter = 0;
            }
            
            if ( counter == 5 ) {
                counter = 0;
                if ( map[y - 1][x - 2] == 0 ) {
                    map[y - 1][x - 2] = 6;
                } else if ( map[y - 1][x - 2] == 1) {
                    map[y + 1][x - 2] = 5;
                }
            }
            
            if ( ( y == sizeY / 4 && y == sizeX / 4 ) ||
                 ( y == 3 * ( sizeY / 4 ) && x == 3 * ( sizeX / 4 ) ) ||
                 ( y == 3 * ( sizeY / 4 ) && x == ( sizeX / 4 ) ) ||
                 ( y == ( sizeY / 4 ) && x == ( sizeX / 4 ) ) )
            {
                if ( map[y][x] == 1 ) {
                    map[y][x] = 31;
                } else {
                    map[y][x] = 30;
                }
            }
        }
    }
    
    for ( int y = 1; y != sizeY - 1; y++ ) {
        for ( int x = 1; x != sizeX - 1; x++ ) {
            if ( map[y][x] == 1 ) {
                continue;
            }
            
            int alive = checkAlive( x, y );
            if ( alive == 0 ) {
                map[y][x] = 3;
            }
        }
    }
}
