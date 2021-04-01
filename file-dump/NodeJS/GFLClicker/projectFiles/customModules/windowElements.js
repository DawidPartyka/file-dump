const robot = require('robotjs');
const random = require('random');

function Coordinate(x, y){
  this.x = x;
  this.y = y;
}

class Clickable{
  constructor(id, tl, br, h, w) {
    this.id = id;
    this.position = {
      tl: tl,
      br: br
    };
    this.height = h;
    this.width = w;
    this.clickPosReg = [];
  }
  
  moveMouseToPoint(){
    const pos = this.antiPixelTracePos();
    console.log(`Moving to position:`);
    console.log(pos);
    robot.moveMouse(pos.x, pos.y);
  }

  clickElement(){
    robot.mouseClick();
  }

  action(delay, callback, x){
    console.log(`Action with delay: ${delay}`);
    async function call(pos){
      console.log(pos);
      robot.moveMouse(pos.x, pos.y);

      const promise = new Promise((resolve, rej) => {
        setTimeout(function(){
          robot.mouseClick();
          resolve(true);
        }, delay);                          //Waiting the given amount of time to 'Click'
      });

      let fin = await promise;              //Wait for the resolve of the promise
    }

    let pos = this.antiPixelTracePos();

    if(x !== undefined)
      pos = x;

    call(pos).then(function(){
      console.log('Action performed');
      callback();
    });

  }

  getClickPos(){
    const short = this.position;
    return new Coordinate(Math.ceil(short.tl.x + this.width/2), Math.ceil(short.tl.y + this.height/2));
  }

  antiPixelTracePos(){
    const short = this.position.tl;
    return new Coordinate(short.x + random.int(0, Math.ceil(this.width)), short.y + random.int(0, Math.ceil(this.height)));
  }

}

class UniqueButton extends Clickable{
  constructor(windowVal, marLeft, marTop, vW, vH){
    const width   = vW * windowVal.width,
          height  = vH * windowVal.height,
          tl = new Coordinate(windowVal.position.tl.x + marLeft * windowVal.width, windowVal.position.tl.y + marTop * windowVal.height),
          br = new Coordinate(tl.x + width, tl.y + height);

    super(0, tl, br, height, width);
  }
}

class LogisticMission extends Clickable{
  constructor(x, windowVal){
    const vH      = 0.08557,
          vW      = 0.121354,
          marLeft = 0.32173,
          marTop  = 0.870705,
          width   = vW * windowVal.width,
          height  = vH * windowVal.height,
          gap     = 0.041392 * windowVal.width,
          tl = new Coordinate(windowVal.position.tl.x + marLeft * windowVal.width + (width + gap) * x * 1.1, windowVal.position.tl.y + marTop * windowVal.height),
          br = new Coordinate(tl.x + width, tl.y + height);

    super(x, tl, br, height, width);
  }
}

class LogisticChapter extends Clickable{
  constructor(windowVal, marLeft, marTop, vW, vH){
    const width   = vW * windowVal.width,
          height  = vH * windowVal.height,
          tl = new Coordinate(windowVal.position.tl.x + marLeft * windowVal.width, windowVal.position.tl.y + marTop * windowVal.height),
          br = new Coordinate(tl.x + width, tl.y + height);

    super(0, tl, br, height, width);
    this.gap = 0.0151006 * windowVal.height;
  }

  scroll(direction, mission, callback){
    if(mission > 5){
      if(direction === 'up'){
        let start = new Coordinate(this.antiPixelTracePos().x, this.getClickPos().y);
        let end = new Coordinate(start.x, start.y+this.height*5);

        const repeatScroll = (repeat) => {
          if(repeat > 0){
            repeat--;
            robot.moveMouse(start.x, start.y);

            setTimeout(() => {
              robot.mouseToggle("down");
              robot.moveMouseSmooth(end.x, end.y);

              setTimeout(() => {
                robot.mouseToggle("up");
                repeatScroll(repeat);
              }, random.int(2000, 3000));
            }, random.int(1000, 2000));
          }
          else{
            callback();
          }
        }

        repeatScroll(2);
      }
      else{
        const start = new Coordinate(this.antiPixelTracePos().x, this.calcChapterPos(5).y);
        const end = new Coordinate(start.x, this.getClickPos().y);

        robot.moveMouse(start.x, start.y);

        setTimeout(() => {
          robot.mouseToggle("down");
          robot.moveMouseSmooth(end.x, end.y - (this.height * 1.8));

          setTimeout(() => {
            robot.mouseToggle("up");
            callback();
          }, random.int(3000, 4000));
        }, random.int(1000, 2000));
      }


    }
    else{
      callback();
    }
  }
  moveMouseToChapter(x){
    let pos = new Coordinate(this.antiPixelTracePos().x, this.calcChapterPos(x).y);
    robot.moveMouse(pos.x, pos.y);
  }
  calcChapterPos(x){
    let ntl = this.position.tl.y;

    if(x > 0){
      ntl = this.position.tl.y + (( this.height + (this.gap * 1.7) ) * x);
    }

    return new Coordinate(Math.ceil(this.position.tl.x + this.width/2), Math.ceil(ntl + this.height/2));
  }
}

class Window{
  constructor(tlx, tly, brx, bry) {
    this.position = {
      tl: new Coordinate(tlx, tly),
      br: new Coordinate(brx, bry)
    };
    this.height = bry - tly;
    this.width = brx - tlx;
    this.basicUnit = this.height/this.width;

    this.contents = {
      logistics: {
        main: {},
        chapter: {},
        mission: [],
        confirmEchelon: {},
        repeat_mission: {},
        returnToMain: {}
      },
      unique:{
        combat: {}
      }
    };
  }
  init(){
    this.contents.unique.combat             = new UniqueButton(this, 0.632173, 0.609060, 0.187206, 0.127516);
    this.contents.logistics.main            = new UniqueButton(this, 0.007246, 0.278523, 0.129821, 0.098993);
    this.contents.logistics.confirmEchelon  = new UniqueButton(this, 0.849482, 0.857382, 0.127939, 0.082214);
    this.contents.logistics.chapter         = new LogisticChapter(this, 0.144873, 0.144295, 0.098777, 0.115771);
    this.contents.logistics.returnToMain    = new UniqueButton(this, 0.038570, 0.036912, 0.069614, 0.067114);
    //this.contents.logistics.chapter = new LogisticChapter(this, 0.295390, 0.139261, 0.096895, 0.112416);

    for(let i = 0; i < 4; i++){
      this.contents.logistics.mission.push(new LogisticMission(i, this));
    }
  }
}

module.exports = Window;
