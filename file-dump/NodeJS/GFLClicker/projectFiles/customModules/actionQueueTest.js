const robot = require('robotjs');
const random = require('random');

async function uniAsyncCall(delay, callback){
  return new Promise((resolve, rej) => {
    setTimeout(function(){
      callback();
      resolve(true);
    }, delay)                           //Waiting the given amount of time to 'Click'
  });
}

class performActions{
  constructor(mission, chapter, time){
    this.chapterPosition = 'up';
  }
  static getToLogistics(winCon, direction, callback){ //Direction = enter / exit the logistics screen (winCon NEEDS TO BE PASSED Win.contents!!!!!!!!! (Might change winCon to {contents} and pass whole Win))
    if(direction === 'enter'){
      const asyncTasks = async() => {
        winCon.unique.combat.moveMouseToPoint();      //Get to Logistic missions screen
        await uniAsyncCall(random.int(1000, 2000), () => { winCon.logistics.main.clickElement(); });

        winCon.logistics.main.moveMouseToPoint();     //Get to Combat screen
        await uniAsyncCall(random.int(6000, 7800), () => { winCon.unique.combat.clickElement(); }).then(() => { callback(); });
      }

      asyncTasks();
    }

    else if(direction === 'exit'){
      const mainActions = () => {
        winCon.logistics.chapter.action(random.int(2000, 3000), async() => {
          robot.mouseClick();
          winCon.logistics.returnToMain.moveMouseToPoint();

          await uniAsyncCall(random.int(2000, 3000), () => { winCon.logistics.returnToMain.clickElement(); }).then(() => { callback(); });
        }, winCon.logistics.chapter.calcChapterPos(0));
      }

      if(this.chapterPosition === 'down'){
        winCon.logistics.chapter.scroll('up', 9, () => {
          mainActions();
        });
      }
      else{
        mainActions();
      }
    }
  }

  deploy(arr, Win){
    const deployEchelon = (arr) => {
      const tmp = arr[0];
      let missionChapter = tmp.mission.split('-').map((item) => { return parseInt(item) });
      missionChapter[1]--; //Diminished by 1; Input is 1-4. Array is indexed 0-3;

      //..next recursive call or finish the algorithm
      const decision = () => {
        arr.shift();

        if(arr.length)
          deployEchelon(arr);
        else
          performActions.getToLogistics(Win.contents, 'exit', () => {
            //..Do smth else. Go to sleep etc
            console.log('Finished deployment');
          });
      }

      //..Choose mission and then deploy
      const missionStart = async() => {
        Win.contents.logistics.mission[missionChapter[1]].moveMouseToPoint();
        await uniAsyncCall(random.int(1000, 3000), () => { robot.mouseClick(); })

        Win.contents.logistics.confirmEchelon.moveMouseToPoint();
        await uniAsyncCall(random.int(2500, 3500), () => { robot.mouseClick(); }).then(() => { decision(); });
      }

      const chapterSelect = async(chapter) => {
        Win.contents.logistics.chapter.moveMouseToChapter(chapter);
        await uniAsyncCall(random.int(1000, 3000), () => { robot.mouseClick(); }).then(() => { missionStart(); });
      }

      console.log(`chapter: ${missionChapter[0]}`);

      if(missionChapter[0] < 6){
        if(this.chapterPosition === 'up'){
          console.log('not scrolling up');
          chapterSelect(missionChapter[0]);
        }
        else{
          //..scroll and deploy
          console.log('scrolling up');
          Win.contents.logistics.chapter.scroll('up', 9, () => {
            this.chapterPosition = 'up';
            chapterSelect(missionChapter[0]);
          });
        }
      }
      else{
        missionChapter[0] -= 6; //Diminished. In "down" position chapter 6 is in position 0

        if(this.chapterPosition === 'down'){
          //..deploy as is
          console.log('not scrolling down');
          chapterSelect(missionChapter[0]);
        }
        else{
          //..scroll and deploy
          console.log('scrolling down');
          Win.contents.logistics.chapter.scroll('down', 9, () => {
            this.chapterPosition = 'down';
            chapterSelect(missionChapter[0]);
          });
        }
      }
    }

    performActions.getToLogistics(Win.contents, 'enter', () => {
      deployEchelon(arr);
    })
  }
}

let queue = {
  queueArr: [],
  timeNow: () => { return Date.now },
  addToQueue: (obj) => {
    this.queueArr.push(obj);
  },
  sortQueue: () => {

  },
  wakeUp: (delay, callback) => {

  },
  testMethod: (Win) => {
    performActions.getToLogistics(Win.contents, 'enter', () => {
      console.log('entered');
      performActions.getToLogistics(Win.contents, 'exit', () => {
        console.log('left');
      });
    });
  },
  actions: new performActions()
}




module.exports = queue;
