using Godot;
using MyDemo;
using System;

public partial class Player : Area2D
{
	[Export]
	public float Speed { get; set; } = 400f;

	public Vector2 ScreenSize { get; set; }

	public override void _Ready()
	{
		// Initialization code can go here if needed
	}

	public override void _Process(double delta) 
	{
		var velocity = Vector2.Zero;
		if (Input.IsActionPressed(GlobalContext.MOVE_RIGHT)) 
		{
			velocity.X += 1;
		}
		if (Input.IsActionPressed(GlobalContext.MOVE_LEFT)) 
		{
			velocity.X -= 1;
		}
		if(Input.IsActionPressed(GlobalContext.MOVE_UP)) 
		{
			velocity.Y -= 1;
		}
		if (Input.IsActionPressed(GlobalContext.MOVE_DOWN)) 
		{
			velocity.Y += 1;
		}
		var animatedSprite = GetNode<AnimatedSprite2D>(GlobalContext.PLAYER_ANIMATED_SPRITE);
		if (velocity.Length() > 0) 
		{
			// 归一化速度向量防止斜向移动偏快，且乘以Speed和delta时间保证平滑移动
			velocity = velocity.Normalized() * Speed * (float)delta;
			Position += velocity;
			// 处理溢出屏幕边界
			Position = new Vector2( x:Mathf.Clamp(Position.X, 0, ScreenSize.X),
									y:Mathf.Clamp(Position.Y, 0, ScreenSize.Y));
			// 更新动画
			UpdatePlayerAnimation(velocity, animatedSprite);
			return;
		}
		UpdatePlayerAnimation(velocity, animatedSprite);
	}
	private void UpdatePlayerAnimation(Vector2 vector, AnimatedSprite2D animatedSprite)
	{
		if(vector.X != 0) 
		{
			animatedSprite.Animation = GlobalContext.PLAYER_ANIMATED_WALK;
			animatedSprite.FlipH = vector.X < 0;
			animatedSprite.FlipV = vector.Y > 0;
		}
		// 只有仅在Y轴上移动时才播放向上动画
		if(vector.Y != 0 && vector.X == 0) 
		{
			animatedSprite.Animation = GlobalContext.PLAYER_ANIMATED_UP;
			animatedSprite.FlipH = false; // 确保向上动画不翻转
			animatedSprite.FlipV = vector.Y > 0; // 如果向下移动则翻转
		}
		if (vector.Length() == 0) 
		{
			animatedSprite.Stop();
			return;
		}
		animatedSprite.Play();
	}
}
