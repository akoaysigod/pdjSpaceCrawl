function createPlayerUI() {
	%playerUIWindow = new SceneWindow( UIWindow ) {
		Profile = GuiDefaultProfile;
		size = "100 75";
		sosition = "0 0";
	};

	%playerUI = new Scene( UIScene );

	%health = new Sprite( HealthBar ) {
		image = "gameModule:healthBar";
		size = "48 0.7";
		position = "-0.2 -34.3";
		SceneLayer = 1;
	};
	%health.start = -0.2;
	%health.maxSize = 48;
	%playerUI.add( %health );

	%healthGraphic = new Sprite() {
		image = "gameModule:healthGraphic";
		size = "51 2.5";
		position = "0 -34";
		SceneLayer = 5;
	};
	%playerUI.add( %healthGraphic );
	%health.updateHealth( -1000 );

	%laser = new Sprite( LaserBar ) {
		image = "gameModule:fuelBar";
		size = "48 0.7";
		position = "-0.2 -33.3";
		SceneLayer = 0;
	};
	%laser.start = -0.2;
	%laser.maxSize = 48;
	%laser.health = 100;
	%laser.setBlendColor( 0, 1.0, 0 );
	%laser.canShoot = true;
	%playerUI.add( %laser );

	%fuel = new Sprite( FuelBar ) {
		image = "gameModule:fuelBar";
		size = "0 1.5";
		position = "-44 29";
		SceneLayer = 0;
		angle = 90;
	};
	%fuel.start = 29;
	%fuel.maxSize = "11.3";
	%playerUI.add( %fuel );

	%fuelBack = new Sprite() {
		image = "gameModule:healthGraphic";
		size = "12 2.5";
		position = "-44 29";
		SceneLayer = 1;
		angle = 90;
	};
	%playerUI.add( %fuelBack );

	%fuelLabel = new ImageFont() {
		image = "gameModule:font";
		FontSize = "2";
		text = "FUEL";
		position = "-44 21";
		SceneLayer = 0;
	};
	%playerUI.add( %fuelLabel );

	%mother = new Sprite( MotherHealth ) {
		image = "gameModule:healthBar";
		size = "11.3 1.5";
		position = "-44 13";
		SceneLayer = 0;
		angle = 90;
	};
	%mother.start = 13;
	%mother.maxSize = "11.3";
	%playerUI.add( %mother );

	%motherBack = new Sprite() {
		image = "gameModule:healthGraphic";
		size = "12 2.5";
		position = "-44 13";
		SceneLayer = 1;
		angle = 90;
	};
	%playerUI.add( %motherBack );

	%motherLabel = new ImageFont() {
		image = "gameModule:font";
		FontSize = "2";
		text = "ARMOR";
		position = "-44 6";
		SceneLayer = 0;
	};
	%playerUI.add( %motherLabel );

	%money = new ImageFont( MoneyLabel ) {
		image = "gameModule:font";
		FontSize = 2;
		text = "$: ";
		position = "35 35";
		SceneLayer = 0;
		textAlignment = "center";
	};
	%money.updateMoney( 0 );
	%playerUI.add( %money );

	%playerUIWindow.setScene( %playerUI );
	Canvas.add( %playerUIWindow );
}

function MoneyLabel::updateMoney( %this, %change ) {
	Mothership.money += %change;

	if ( isObject( Monies ) ) {
		Monies.delete();
	}

	%money = new ImageFont( Monies ) {
		image = "gameModule:font";
		FontSize = 2;
		text = Mothership.money;
		position = "50 36";
		SceneLayer = 0;
		textAlignment = "right";
	};
	UIScene.add( %money );
}

function MotherHealth::flasher( %this ) {
	if ( Alert.getVisible() ) {
		Alert.setVisible( false );
	} else {
		Alert.setVisible( true );
	}
	%this.flasher = %this.schedule( 1000, flasher );
}

function MotherHealth::setupAlert( %this ) {
	%test = new ImageFont( Alert )  {
		image = "gameModule:font";
		Text = "Mothership under attack!";
		FontSize = "3";
		Position = "0 33";
		SceneLayer = "0";
		BlendColor = "255, 0, 0";
		TextAlignment = "center";
		LifeTime = 10;
	};
	UIScene.add( %test );
	%this.flasher = %this.schedule( 1000, flasher );
}

function MotherHealth::updateHealth( %this, %change ) {
	if ( %change == -1000 ) {
		Mothership.health = Mothership.health;
	} else {
		Mothership.health = Mothership.health - %change;
	}

	if ( Mothership.health <= 0 ) {
		gameOver();
		return;
	}
	
	%currentHealth = Mothership.health;
	%size = %currentHealth / 100;
	%sizeX = %size * %this.maxSize;

	if ( %change == 0 ) {
		%sizeX = %this.maxSize;
	}

	%y = %this.start - ( ( %this.maxSize  - %sizeX ) / 2 );

	%this.setPositionY( %y );
	%this.setSizeX( %sizeX );
}

function HealthBar::updateHealth( %this, %change ) {
	if ( %change == -1000 ) {
		Ship.health = Ship.health;
	} else {
		Ship.health = Ship.health - %change;
	}
	
	%currentHealth = Ship.health;
	%size = %currentHealth / 100;
	%sizeX = %size * %this.maxSize;

	if ( %change == 0 ) {
		%sizeX = %this.maxSize;
	}

	%x = %this.start - ( ( %this.maxSize - %sizeX ) / 2 );

	%this.setPositionX( %x );
	%this.setSizeX( %sizeX );
}

function FuelBar::updateFuel( %this ) {
	if ( Mothership.amount < 4 ) {
		%size = Mothership.fuel / 4;
		%sizeX = %size * %this.maxSize;

		%y = %this.start - ( ( %this.maxSize  - %sizeX ) / 2 );

		%this.setPositionY( %y );
		%this.setSizeX( %sizeX ); 
	}
}

function LaserBar::recharge( %this ) {
	if ( %this.health >= 100 ) {
		%this.health = 100;
		%this.updateLaser( 0 );
		%this.stopTimer();
		%this.canShoot = true;
		return;
	}

	%this.health += 1;
	%this.updateLaser( -1 );
}

function LaserBar::updateLaser( %this, %change ) {
	%this.health = %this.health - %change;

	%size = %this.health / 100;
	%sizeX = %size * %this.maxSize;

	if ( %change == 0 ) {
		%sizeX = %this.maxSize;
	}

	%x = %this.start - ( ( %this.maxSize - %sizeX ) / 2 );

	%this.setPositionX( %x );
	%this.setSizeX( %sizeX );

	if ( %this.health < 100 ) {
		%this.startTimer( recharge, Ship.rechargeRate, 0 );
	}

	if ( %this.health <= 0 ) {
		%this.canShoot = false;
	}
}

function removePlayerUI() {
	UIScene.clear( true );
	UIScene.delete();
	UIWindow.delete();
}