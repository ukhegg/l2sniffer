using Interceptor;
using Keyboard;

namespace L2Clicker;

public class L2WindowController
{
    private readonly L2Process _l2Process;

    public L2WindowController(L2Process l2Process)
    {
        _l2Process = l2Process;
    }

    public void Sit()
    {
        _l2Process.MainWindow.SendChatCommand("/sit");
        Thread.Sleep(2500);
    }

    public void Stand()
    {
        _l2Process.MainWindow.SendChatCommand("/stand");
        Thread.Sleep(2500);
    }

    public void Attack(bool force)
    {
        _l2Process.MainWindow.SendChatCommand(force ? "/attackforce" : "/attack");
    }

    public void AttackStand()
    {
        _l2Process.MainWindow.SendChatCommand("/attackstand");
    }

    public void Target(string target)
    {
        _l2Process.MainWindow.SendChatCommand($"/target {target}");
    }

    public void NextTarget()
    {
        _l2Process.MainWindow.SendChatCommand("/nexttarget");
    }

    public void Assist()
    {
        _l2Process.MainWindow.SendChatCommand("/assist");
    }

    public void PartyInvite()
    {
        _l2Process.MainWindow.SendChatCommand("/invite");
    }

    public void Leave()
    {
        _l2Process.MainWindow.SendChatCommand("/leave");
    }

    public void PartyDismiss(string name)
    {
        _l2Process.MainWindow.SendChatCommand($"/dismiss {name}");
    }

    public void Pickup()
    {
        _l2Process.MainWindow.SendChatCommand("/pickup");
    }

    public void SocialNo()
    {
        _l2Process.MainWindow.SendChatCommand("/socialno");
        Thread.Sleep(1000);
    }

    public void SocialYes()
    {
        _l2Process.MainWindow.SendChatCommand("/socialyes");
        Thread.Sleep(1000);
    }

    public void SocialBow()
    {
        _l2Process.MainWindow.SendChatCommand("/socialbow");
        Thread.Sleep(1000);
    }

    public void SocialUnaware()
    {
        _l2Process.MainWindow.SendChatCommand("/socialunaware");
        Thread.Sleep(1000);
    }

    public void SocialLaugh()
    {
        _l2Process.MainWindow.SendChatCommand("/sociallaugh");
        Thread.Sleep(1000);
    }

    public void SocialHello()
    {
        _l2Process.MainWindow.SendChatCommand("/socialhello");
        Thread.Sleep(1000);
    }

    public void SocialVictory()
    {
        _l2Process.MainWindow.SendChatCommand("/socialvictory");
        Thread.Sleep(1000);
    }

    public void SocialCharge()
    {
        _l2Process.MainWindow.SendChatCommand("/socialcharge");
        Thread.Sleep(1000);
    }

    public void SocialDance()
    {
        _l2Process.MainWindow.SendChatCommand("/socialdance");
        Thread.Sleep(4000);
    }

    public void SocialSad()
    {
        _l2Process.MainWindow.SendChatCommand("/socialsad");
        Thread.Sleep(1000);
    }

    public void SocialApplause()
    {
        _l2Process.MainWindow.SendChatCommand("/socialapplause");
        Thread.Sleep(1000);
    }

    public void SocialWaiting()
    {
        _l2Process.MainWindow.SendChatCommand("/socialwaiting");
        Thread.Sleep(1000);
    }

    public void SocialCharm()
    {
        _l2Process.MainWindow.SendChatCommand("/charm");
        Thread.Sleep(1000);
    }

    public void ChangeShortcutPanel(int index)
    {
        _l2Process.MainWindow.SendModifiedText(index.ToString(), holdAlt: true, holdShift: false);
    }
}