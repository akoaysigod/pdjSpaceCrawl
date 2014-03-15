function createWorld() { 
	exec( "./mothership.cs" );
	exec( "./spaceship.cs" );
	exec( "./enemyShip.cs" );
	exec( "./stationGun.cs" );
	exec( "./item.cs" );
	exec( "./enemySpawnPoint.cs" );
	exec( "./background.cs" );
	
	createBackground();


	$height = 100;
	$width = 200;
	$scaleFactor = 2;
	
	%test = new LevelGen( GenLevel );
	%test.initLevel( $width, $height, 40 );
	%test.automata( 6, 3, 20 );
	%test.automata( 5, 5, 2 );
	%test.finish();
	//genLand();
	//auto( 6, 3, 20 );
	//auto( 5, 5, 1 );
	%test.genSprites();	
	%test.genEnemies();
	
	placeMothership();
} 

function GenLevel::genSprites( %this ) {
	%xPos = 0;
	%xStart = %xPos;
	%yPos = 0;
	%blockSize = $scaleFactor;
	%size = %blockSize SPC %blockSize;
	%moveX = %blockSize;
	%moveY = %blockSize;
	%simulate = 1;

	%placeOne = false;
	%placeTwo = false;
	%placeThree = false;
	%placeFour = false;
	%placeCount = 95;

	for ( %y = 0; %y != $height; %y++ ) {
		for ( %x = 0; %x != $width; %x++ ) {
			%cell = %this.getVal( %x, %y );

			if ( %cell == 3 ) {
				%item = createItem();
				%chance = getRandom( 0, 99 );
				if ( !%placeOne && %chance > %placeCount ) {
					%item.position = %xPos SPC %yPos;
					%placeCount = 95;
					%placeOne = true;
					GameScene.add( %item );
				} else if ( !%placeTwo && %chance > %placeCount ) {
					if ( %x > $width / 2 && %y < $height / 2 ) {
						%item.position = %xPos SPC %yPos;
						%placeCount = 95;
						%placeTwo = true;
						GameScene.add( %item );
					}
				} else if ( !%placeThree && %chance > %placeCount ) {
					if ( %x < $width / 2 && %y > $height / 2 ) {
						%item.position = %xPos SPC %yPos;
						%placeCount = 95;
						%placeThree = true;
						GameScene.add( %item );
					}
				} else if ( !%placeFour && %chance > %placeCount ) {
					if ( %x > $width / 2 && %y > $height / 2 ) {
						%item.position = %xPos SPC %yPos;
						%placeFour = true;
						GameScene.add( %item );
					}
				}
			}
			%placeCount -= 1;

			if ( %cell == 1 || %cell == 11 || %cell == 31 ) {
				%block = new Sprite();
				%block.name = "block";
				%block.Image = "gameModule:rockText";
				%block.setSceneGroup( 10 );
				%block.setCollisionGroups( 10 );
				%block.setCollisionLayers( 10 );
				%block.Size = %size;
				%block.Position = %xPos SPC %yPos;
				%block.SceneLayer = 10;
				
				if ( %simulate ) {
					%block.setBodyType( static );
					%block.createPolygonBoxCollisionShape( %size );
					%block.setDefaultRestitution( 0.5 );
				}
				GameScene.add( %block );
			} 

			if ( ( %cell == 5 || %cell == 6 ) && %y > 10 ) {
				%gun = createStationGun( %cell );
				%gun.Position = %xPos SPC %yPos;
				GameScene.add( %gun );
			}

			if ( %cell == 30 || %cell == 31 ) {
				createEnemySpawnPoint( %xPos, %yPos );
			}
			%xPos += %moveX;	
		}
		%xPos = %xStart;
		%yPos += %moveY;
	}
}

function GenLevel::genEnemies() {
	%sizeX = $width * $scaleFactor;
	%sizeY = $height * $scaleFactor;

	for ( %i = 0; %i != 4; %i++ ) {
		%end = getRandom( 3, 5 );
		for ( %k = 0; %k != %end; %k++ ) {
			%enemy = createEnemyShip();

			if ( %i == 0 ) {
				%xPos = getRandom( 10, %sizeX / 2 );
				%yPos = getRandom( 10, %sizeY / 2 );
			} else if ( %i == 1 ) {
				%xPos = getRandom( %sizeX / 2, %sizeX );
				%yPos = getRandom( %sizeY / 2, %sizeY );
			} else if ( %i == 2 ) {
				%xPos = getRandom( 10, %sizeX / 2 );
				%yPos = getRandom( %sizeY / 2, %sizeY );
			} else if ( %i == 3 ) {
				%xPos = getRandom( %sizeX / 2, %sizeX );
				%yPos = getRandom( 10, %sizeY / 2 );
			}

			%enemy.Position = %xPos SPC %yPos;
			GameScene.add( %enemy );
		}
	}
}

function placeMothership() {
	%start = getRandom( 0, $width );
	
	%mothership = createMothership();
	%mothership.position = %start SPC ( Mothership.getHeight() / 2 + ( $height * $scaleFactor ) - 1.1 );	GameScene.add( %mothership );
	
	%mothership.createDropBox();
	
	%playership = createSpaceShip();
	%controls = ShipControls.createInstance();
	%playership.addBehavior( %controls );
	%playership.Position = %mothership.getPosition();//%start SPC ( $height * $scaleFactor ) + 25;
	
	GameScene.add( %playership );
	
	Window.mount( %playership, 0, 0, 10, true, false );
	Window.setViewLimitOn( 0, 0, $width * $scaleFactor, ( $height * $scaleFactor ) + 75);
	
	%item = createItem();
	%item.setPositionX( %start + 20 );
	%item.setPositionY( 200 + 15 );
	GameScene.add( %item );
}










	
/*
//old slow level generation 

function genLand() {
	$width = 100;
	$height = 100;
	$map[$height, $width] = 1;
	
	%live = 50;

	for ( %i = 0; %i != $height; %i++ ) {
		for ( %k = 0; %k != $width; %k++ ) {
			if ( getRandom( 1, 100 ) < %live ) {
				$map[%i, %k] = 1;
			} else {	
				$map[%i, %k] = 0;
			}
		}
	}
}

function auto( %birth, %survive, %iterations ) {
	for ( %j = 0; %j != %iterations; %j++ ) {
		for ( %i = 1; %i != $height; %i++ ) {
			for ( %k = 1; %k != $width; %k++ ) { 
				%living = alive( %i, %k );
				if ( $map[%i, %k] == 1 ) {
					if ( %living < %survive ) {
						$mapTwo[%i, %k] = 0;
					} else { 
						$mapTwo[%i, %k] = 1;	
					}
				} else {
					if ( %living > %birth ) {
						$mapTwo[%i, %k] = 1;
					} else {
						$mapTwo[%i, %k] = 0;
					}	
				}
			}
		}
		mapToMap();
	}
}

function mapToMap() {
	for ( %i = 1; %i != $height; %i++ ) {
		for ( %k = 1; %k != $width; %k++ ) {
			$map[%i, %k] = $mapTwo[%i, %k];
		}
	}
}

function alive( %x, %y ) {
	%ret = 0;
	%ret += $map[%x + 1, %y];
	%ret += $map[%x + 1, %y - 1];
	%ret += $map[%x + 1, %y + 1];
	%ret += $map[%x - 1, %y];
	%ret += $map[%x - 1, %y + 1];
	%ret += $map[%x - 1, %y - 1];
	%ret += $map[%x, %y + 1];
	%ret += $map[ %x, %y - 1];
	return %ret;
}

function genSprites() {
	%x = -100;
	%xStart = %x;
	%y = 0;
	%blockSize = 2;
	%size = %blockSize SPC %blockSize;
	%moveX = %blockSize;
	%moveY = %blockSize;
	%simulate = 1;
	
	for ( %i = 0; %i != $height; %i++ ) {
		for ( %k = 0; %k != $width; %k++ ) {
			if ( $map[%i, %k] == 1 && alive( %i, %k ) <= 1 ) {
				$map[%i, %k] = 0;
			} else if ( $map[%i, %k] == 1 ) {
				%block = new Sprite();
				%block.Image = "gameModule:rockText";
				%block.setSceneGroup( 10 );
				%block.setCollisionGroups( 10 );
				%block.setCollisionLayers( 10 );
				%block.Size = %size;
				%block.Position = %x SPC %y;
				%block.SceneLayer = 1;
				
				if ( %simulate && alive( %i, %k ) >= 2 ) {
					%block.setBodyType( static );
					%block.createPolygonBoxCollisionShape( %size );
					%block.setDefaultRestitution( 0.5 );
				}
				
				GameScene.add( %block );
			}
			%x += %moveX;	
		}
		%x = %xStart;
		%y += %moveY;
	}
}
*/
	
