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

	%playership = createSpaceShip();

	%test = new LevelGen( GenLevel );
	%test.initLevel( $width, $height, 35 );
	%test.automata( 6, 3, 20 );
	%test.automata( 5, 5, 1 );
	%test.finish();
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

	for ( %y = 0; %y != $height - 1; %y++ ) {
		for ( %x = 0; %x != $width; %x++ ) {
			%cell = %this.getVal( %x, %y );

			if ( %cell == 1 || %cell == 11 || %cell == 31 || ( %cell == 5 && World.planetID == 0 ) ) {
				%block = new Sprite();
				%block.name = "block";
				if ( getRandom( 0, 1 ) ) {
					%block.Image = "gameModule:rockTextOne";
				} else {
					%block.image = "gameModule:rockTextTwo";
				}
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

function GenLevel::quadChooser( %this, %quad ) {
	if ( %quad == 0 ) {
		if ( !( %x < $width / 2 && %y < $height / 2 ) ) {
			return true;
		}
	} else if ( %quad == 1 ) {
		if ( !( %x > $width / 2 && %y < $height / 2 ) ) {
			return true;
		}
	} else if ( %quad == 2 ) {
		if ( !( %x < $width / 2 && %y > $height / 2 ) ) { 
			return true;
		}
	} else if ( %quad == 3 ) {
		if ( !( %x > $width / 2 && %y > $height / 2 ) ) {
			return true;
		}
	}
	return false;
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

	%hasPlacedSpecial = false;
	%specialQuad = getRandom( 0, 3 );

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
					%xPos += %moveX;
					continue;
				} else if ( !%placeTwo && %chance > %placeCount ) {
					if ( %x - %min > $width / 2 && %y + %min < $height / 2 ) {
						%item = createItem( "fuel" );
						%item.position = %xPos SPC %yPos;
						%placeCount = 95;
						%placeTwo = true;
						GameScene.add( %item );
						%xPos += %moveX;
						continue;
					}
				} else if ( !%placeThree && %chance > %placeCount ) {
					if ( %x + %min < $width / 2 && %y - %min > $height / 2 ) {
						%item = createItem( "fuel" );
						%item.position = %xPos SPC %yPos;
						%placeCount = 95;
						%placeThree = true;
						GameScene.add( %item );
						%xPos += %moveX;
						continue;
					}
				} else if ( !%placeFour && %chance > %placeCount ) {
					if ( %x - %min > $width / 2 && %y - %min > $height / 2 ) {
						%item = createItem( "fuel" );
						%item.position = %xPos SPC %yPos;
						%placeFour = true;
						GameScene.add( %item );
						%xPos += %moveX;
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
							%xPos += %moveX;
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
							%xPos += %moveX;
							continue;
						}
					}
				}

				if ( Window.planetID > 0 ) {
					if ( %coin && %chance > 95 ) {
						if ( !%this.quadChooser( %quad ) ) {
							%xPos += %moveX;
							continue;
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
						%part.positon = %xPos SPC %yPos;
						GameScene.add( %part );
					}
					if ( !%hasPlacedSpecial ) {
						if ( !%this.quadChooser( %specialQuad ) ) {
							%xPos += %moveX;
							continue;
						}

						if ( %chance > 95 ) {
							%hasPlacedSpecial = true;

							if ( !Ship.hasReverseThruster ) {
								%special = createItem( "reverseThrusters" );
							} else if ( !Ship.hasSpecial ) {
								%special = createItem( "missiles" );
							} else if ( !Ship.hasBoosters ) {
								%special = createItem( "boosters" );
							}
							%special.position = %xPos SPC %yPos;
							GameScene.add( %special );
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

function genPlatform() {
	%i = ( $width / 2 ) * $scaleFactor;
	%i -= 20;
	%e = %i + 40;
	%size = $scaleFactor SPC $scaleFactor;

	for ( %i; %i != %e; %i++ ) {
		%block = new Sprite();
		%block.name = "block";
		if ( getRandom( 0, 1 ) ) {
			%block.Image = "gameModule:rockTextOne";
		} else {
			%block.Image = "gameModule:rockTextTwo";
		}
		%block.setSceneGroup( 10 );
		%block.setCollisionGroups( 10 );
		%block.setCollisionLayers( 10 );
		%block.Size = %size;
		%block.Position = %i SPC ( $height * $scaleFactor );
		%block.SceneLayer = 10;

		%block.setBodyType( static );
		%block.createPolygonBoxCollisionShape( %size );
		%block.setDefaultRestitution( 0.5 );

		GameScene.add( %block );
	}
}

function placeMothership() {
	%start = ( $width / 2 ) * $scaleFactor;

	genPlatform();
	
	%mothership = createMothership();
	%mothership.position = %start SPC ( Mothership.getHeight() / 2 + ( $height * $scaleFactor ) + 1 );	GameScene.add( %mothership );
	%mothership.createDropBox();
	
	%controls = ShipControls.createInstance();
	Ship.addBehavior( %controls );
	Ship.Position = %mothership.getPosition();//%start SPC ( $height * $scaleFactor ) + 25;
	GameScene.add( Ship );
	
	Window.mount( Ship, 0, 0, 10, true, false );
	Window.setViewLimitOn( 0, 0, $width * $scaleFactor, ( $height * $scaleFactor ) + 75);

	alxPlay( "gameModule:gameMusic" );
}


