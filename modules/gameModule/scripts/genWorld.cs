function createWorld() { 
	exec( "./mothership.cs" );
	exec( "./spaceship.cs" );
	exec( "./enemyShip.cs" );
	exec( "./stationGun.cs" );
	exec( "./item.cs" );
	exec( "./enemySpawnPoint.cs" );
	exec( "./background.cs" );

	$height = 100;
	$width = 200;
	$scaleFactor = 2;
	createBackground();

	%test = new LevelGen( GenLevel );
	%test.initLevel( $width, $height, 40 );
	%test.automata( 6, 3, 20 );
	%test.automata( 5, 5, 2 );
	%test.finish();
	//genLand();
	//auto( 6, 3, 20 );
	//auto( 5, 5, 1 );
	%test.genSprites();	
	%test.placeItems();
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

	for ( %y = 0; %y != $height; %y++ ) {
		for ( %x = 0; %x != $width; %x++ ) {
			%cell = %this.getVal( %x, %y );

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

function GenLevel::placeItems( %this ) {
	%xPos = 0;
	%xStart = %xPos;
	%yPos = 0;
	%moveX = $scaleFactor;
	%moveY = $scaleFactor;

	%placeOne = false;
	%placeTwo = false;
	%placeThree = false;
	%placeFour = false;
	%shieldsPlaced = false;
	%placeCount = 95;

	%coin = getRandom( 0, 1 );
	%treasure = getRandom( 1, 3 );

	%partOne = Mothership.hasUpgradeOne;
	%partTwo = Mothership.hasUpgradeTwo;
	%partThree = Mothership.hasUpgradeThree;
	%partFour = Mothership.hasUpgradeFour;
	%placePart = getRandom( 1, 3 );
	%quad = getRandom( 0, 3 );

	for ( %y = 0; %y != $height; %y++ ) {
		for ( %x = 0; %x != $width; %x++ ) {
			%cell = %this.getVal( %x, %y );

			%chance = getRandom( 0, 99 );
			%min = getRandom( 10, 20 );
			if ( getRandom( 0, 1 ) ) {
				%min = 0;
			}

			if ( %cell == 3 ) {
				if ( !%placeOne && %chance > %placeCount ) {
					%item = createItem( "fuel" );
					%item.position = %xPos SPC %yPos;
					%placeCount = 95;
					%placeOne = true;
					GameScene.add( %item );
					continue;
				} else if ( !%placeTwo && %chance > %placeCount ) {
					if ( %x - %min > $width / 2 && %y + %min < $height / 2 ) {
						%item = createItem( "fuel" );
						%item.position = %xPos SPC %yPos;
						%placeCount = 95;
						%placeTwo = true;
						GameScene.add( %item );
						continue;
					}
				} else if ( !%placeThree && %chance > %placeCount ) {
					if ( %x + %min < $width / 2 && %y - %min > $height / 2 ) {
						%item = createItem( "fuel" );
						%item.position = %xPos SPC %yPos;
						%placeCount = 95;
						%placeThree = true;
						GameScene.add( %item );
						continue;
					}
				} else if ( !%placeFour && %chance > %placeCount ) {
					if ( %x - %min > $width / 2 && %y - %min > $height / 2 ) {
						%item = createItem( "fuel" );
						%item.position = %xPos SPC %yPos;
						%placeFour = true;
						GameScene.add( %item );
						continue;
					}
				}

				if ( Window.planetID == 0 && !%shieldsPlaced ) {
					if ( %x > $width / 2 && %y > $height / 2 ) {
						if ( %chance > 95 && !%shieldsPlaced ) {
							%shields = createItem( "shields" );
							%shields.position = %xPos SPC %yPos;
							%shieldsPlaced = true;
							GameScene.add( %shields );
							continue;
						}
					}

					if ( %x > $width / 2 && %y < $height / 2 ) {
						if ( %chance > 95 && !%hasUpgradeOne ) {
							%item = createItem( "shipPartOne" );
							%item.position = %xPos SPC %yPos;
							%hasUpgradeOne = true;
							GameScene.hasMotherPart = true;
							GameScene.add( %item );
							continue;
						}
					}
				}

				if ( Window.planetID > 0 ) {
					if ( %coin && %chance > 95 ) {
						if ( %quad == 0 ) {
							if ( !( %x < $width / 2 && %y < $height / 2 ) ) {
								continue;
							}
						} else if ( %quad == 1 ) {
							if ( !( %x > $width / 2 && %y < $height / 2 ) ) {
								continue;
							}
						} else if ( %quad == 2 ) {
							if ( !( %x < $width / 2 && %y > $height / 2 ) ) { 
								continue;
							}
						} else if ( %quad == 3 ) {
							if ( !( %x > $width / 2 && %y > $height / 2 ) ) {
								continue;
							}
						}

						%coin = 0;
						GameScene.hasMotherPart = true;
						switch ( %placePart ) {
							case 1:
								%part = createItem( "shipPartTwo" );
							case 2:
								%part = createItem( "shipPartThree" );
							case 3:
								%part = createItem( "shipPartFour" );
						}
					}
				}
			}
			%placeCount -= 1;
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
	%start = getRandom( 35, $width - 35 );
	
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
	
