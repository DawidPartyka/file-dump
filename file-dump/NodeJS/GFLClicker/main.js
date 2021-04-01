const fs            = require('fs');
const prompt        = require('prompt');
const robot         = require('robotjs');
const random        = require('random');
const windowOpt     = require('./projectFiles/customModules/windowElements.js');
const logisticSheet = require('./projectFiles/customModules/logisticSheet.js');
//const queue         = require('./projectFiles/customModules/actionQueue.js');
const queue         = require('./projectFiles/customModules/actionQueueTest.js');
const configFileDir = 'projectFiles/conf.txt';

function getPositions(x){
  let tl, br;

  console.log('Top left');
  prompt.start();
  prompt.get('tl', function (err, result) {
    tl = robot.getMousePos();

    console.log('\nBottom right');
    prompt.start();
    prompt.get('br', function (err, result) {
      br = robot.getMousePos();

      opt.window = new SetWindowParams(tl, br);

      begin();
    });
  });
}
/*
class Options {
  updateConf(){
    this._window = getPositions();
    //...TODO: Update conf.txt file with this params
  }
  constructor() {
    this.conf = fs.readFileSync('projectFiles/conf.txt', 'utf8');

    if(!this.conf.length){
      getPositions();
    }
    else{
      const config = this.conf.split('\r\n');
      this._window = new SetWindowParams(new Coordinate(config[0], config[1]), new Coordinate(config[2], config[3]));

      console.log('Do you want to update window settings? Y/n');
      prompt.start();
      prompt.get('Confirm', function (err, result) {
        if(result.Confirm === 'Y' || result.Confirm === 'y')
          getPositions();
        else
          begin();
      });
    }
  }
}

let opt = new Options();*/

let par = fs.readFileSync(configFileDir, 'utf8').split('\r\n').map((item) => parseInt(item, 10));

function startConfig(){
  let tl, br;

  console.log('Top left:');
  prompt.start();
  prompt.get('x', function (err, result) {
    tl = robot.getMousePos();

    console.log('Bottom right:');
    prompt.start();
    prompt.get('y', function (err, result) {
      br = robot.getMousePos();

      let txt = `${tl.x}\r\n${tl.y}\r\n${br.x}\r\n${br.y}`;
      fs.writeFile(configFileDir, txt, function (err) {
        if (err) return console.log(err);
        else{
          par = [tl.x, tl.y, br.x, br.y]; //Change par values with the new ones

          begin();
        }
      });
    });
  });
}

function measurments(){
  let tl, br;

  console.log('Top left');
  prompt.start();
  prompt.get('tl', function (err, result) {
    tl = robot.getMousePos();

    console.log('\nBottom right');
    prompt.start();
    prompt.get('br', function (err, result) {
      br = robot.getMousePos();

      console.log('\n');
      console.log(tl);
      console.log(br);

      loopmeasure();
    });
  });
}

function loopmeasure(){
  console.log('\ncontinue? y/n');
  prompt.start();
  prompt.get('cont', function (err, result) {
    if(result.cont !== 'n')
      measurments();
  });
}

function testAim(cls){
  /*const srt = cls.contents;
  const srtL = srt.logistics;

  console.log('start test:\n');

  srt.unique.combat.action(1000, () => {
    srtL.main.action(5000, () => {
      srtL.chapter.action(2000, () => {
        srtL.mission[1].action(2000, () => {
          srtL.confirmEchelon.action(2000, () => {
            srtL.returnToMain.action(2000, () => {
              console.log('Whole process ended');
            });
          });
        });
      }, srtL.chapter.calcChapterPos(2));
    });
  });*/

  function reqCall(arr){
    const click = arr[0].a;
    robot.moveMouse(arr[0].t.x, arr[0].t.y);

    arr.shift();

    prompt.start();
    prompt.get('val', function (err, result) {

        if(click){
          console.log('click?');

          prompt.start();
          prompt.get('val', function (err, result) {
            robot.mouseClick();
            console.log('go?');
            prompt.start();
            prompt.get('val', function (err, result) {
              if(arr.length)
                reqCall(arr);
            });
          });
        }
        else
          if(arr.length)
            reqCall(arr);

    });
  }
  const short = cls.contents;
  const srtL = short.logistics;
  //console.log(short.unique.combat.getClickPos());
  //let aim = [{t: short.unique.combat.getClickPos(), a: true}, {t: short.logistics.main.getClickPos(), a: true}, {t: short.logistics.chapter.getClickPos(), a: false}];
  //let aim = [];

  srtL.chapter.scroll('down', 9, () => {
    srtL.chapter.action(2000, () => {
      srtL.chapter.scroll('up', 9, () => {
        srtL.chapter.action(2000, () => {
                console.log('Whole process ended');
        }, short.logistics.chapter.calcChapterPos(0));
      });
    }, short.logistics.chapter.calcChapterPos(2));
  });

  /*for(let i = 0; i < 6; i++){
    aim.push({t: short.logistics.chapter.calcChapterPos(i), a: false});
  }*/

  /*for(let i = 0; i < 4; i++){
    aim.push({t: short.logistics.mission[i].getClickPos(), a: (i === 3 ? true : false)});
  }

  aim.push({t: short.logistics.confirmEchelon.getClickPos(), a: false});*/

  //reqCall(aim);
}

function deploy(arr, Win){
  console.log('\nDeploying echelon:');
  console.log(arr[0]);
  console.log('\n');

  const srt = Win.contents;
  const srtL = srt.logistics;
  let mission = arr[0].mission.split('-').map((entry) => {return parseInt(entry)});
  mission[1]--;


  srt.unique.combat.action(random.int(1000, 2000), () => {
    srtL.main.action(random.int(6000, 7800), () => {
      srtL.chapter.scroll('down', mission[0], () => {
        srtL.chapter.action(random.int(2000, 3000), () => {
          srtL.mission[mission[1]].action(random.int(2500, 3500), () => {
            srtL.confirmEchelon.action(random.int(2700, 3700), () => {
              //..ADD echelon to queue
              srtL.chapter.scroll('up', mission[0], () => {
                srtL.chapter.action(random.int(2000, 3000), () => {
                  srtL.returnToMain.action(random.int(2000, 3000), () => {
                    console.log('Whole process ended');
                    //..TODO: SAVE RESULTS SOMEWHERE
                    //..THEN:
                    arr.shift();

                    if(arr.length){
                      setTimeout(() => {
                        deploy(arr, Win);
                      }, random.int(2000, 8000))
                    }
                    else{
                      //..GO TO SLEEP OR SMTH
                    }
                  });
                }, srtL.chapter.calcChapterPos(0));
              });
            });
          });
        }, srtL.chapter.calcChapterPos((mission[0] < 6 ? mission[0] : mission[0] - 6)));
      });
    });
  });
}

function signEchelons(ech, num, arr, Win){
  console.log(`\nEchelon ${ech} - choose mission: (chapter-mission_number)`);
  prompt.start();
  prompt.get('mission', function (err, result) {
    arr.push({echelon: ech, mission: result.mission, time: logisticSheet.returnTimer(result.mission)});
    ech++;

    if(ech > num){
      console.log('\nAll echelons signed');
      console.log(arr);

      queue.actions.deploy(arr, Win);
      //deploy(arr, Win);
      //control.createQueue(arr, Win);
    }
    else{
      signEchelons(ech, num, arr, Win);
    }
  });
}

function begin(){
  let Win = new windowOpt(par[0], par[1], par[2], par[3]);

  Win.init();

  /*console.log('\nHow many echelons to deploy? (1-4)');
  prompt.start();
  prompt.get('eNum', function (err, result) {
    signEchelons(1, parseInt(result.eNum, 10), [], Win);
  });*/

  //Win.contents.logistics.chapter.scroll('up', 9, () => {});
  //queue.testMethod(Win);
  //testAim(Win);
  measurments();
}

if(par.every((item) => item === 0)){
  startConfig();
}
else{
  console.log('Do you want to update window position? Y/n');

  prompt.start();
  prompt.get('dec', function (err, result) {
    if(result.dec === 'Y' || result.dec === 'y'){
      startConfig();
    }
    else{
      begin();
    }
  });
}
